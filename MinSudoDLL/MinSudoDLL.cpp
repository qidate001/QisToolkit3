#include <windows.h>
#include <userenv.h>
#include <wtsapi32.h>
#include <string>
#include <vector>
#include <memory>

#include "MinSudoAPI.h"

#pragma comment(lib, "userenv.lib")
#pragma comment(lib, "wtsapi32.lib")
#pragma comment(lib, "advapi32.lib")

// ---------- RAII 句柄管理器 ----------
struct HandleDeleter {
    void operator()(HANDLE h) const {
        if (h && h != INVALID_HANDLE_VALUE) CloseHandle(h);
    }
};
using UniqueHandle = std::unique_ptr<void, HandleDeleter>;

struct ServiceHandleDeleter {
    void operator()(SC_HANDLE h) const {
        if (h) CloseServiceHandle(h);
    }
};
using UniqueServiceHandle = std::unique_ptr<std::remove_pointer<SC_HANDLE>::type, ServiceHandleDeleter>;

// ---------- 辅助函数 ----------
static std::wstring GetWorkingDirectory() {
    WCHAR path[MAX_PATH];
    GetCurrentDirectoryW(MAX_PATH, path);
    return std::wstring(path);
}

static DWORD GetActiveSessionID() {
    DWORD sessionId = 0;
    PWTS_SESSION_INFO pSessionInfo = nullptr;
    DWORD count = 0;
    if (WTSEnumerateSessionsW(WTS_CURRENT_SERVER_HANDLE, 0, 1, &pSessionInfo, &count)) {
        for (DWORD i = 0; i < count; ++i) {
            if (pSessionInfo[i].State == WTSActive) {
                sessionId = pSessionInfo[i].SessionId;
                break;
            }
        }
        WTSFreeMemory(pSessionInfo);
    }
    return sessionId;
}

static bool CreateSystemToken(HANDLE& tokenHandle) {
    tokenHandle = nullptr;
    PWTS_PROCESS_INFOW pProcesses = nullptr;
    DWORD count = 0;
    DWORD lsassPID = 0, winlogonPID = 0;
    DWORD activeSession = GetActiveSessionID();

    if (WTSEnumerateProcessesW(WTS_CURRENT_SERVER_HANDLE, 0, 1, &pProcesses, &count)) {
        for (DWORD i = 0; i < count; ++i) {
            PWTS_PROCESS_INFOW p = &pProcesses[i];
            if (!p->pProcessName || !p->pUserSid) continue;
            if (!IsWellKnownSid(p->pUserSid, WinLocalSystemSid)) continue;

            std::wstring name = p->pProcessName;
            if (lsassPID == 0 && p->SessionId == 0 && _wcsicmp(name.c_str(), L"lsass.exe") == 0)
                lsassPID = p->ProcessId;
            else if (winlogonPID == 0 && (p->SessionId == activeSession || p->SessionId == 0) &&
                _wcsicmp(name.c_str(), L"winlogon.exe") == 0)
                winlogonPID = p->ProcessId;
        }
        WTSFreeMemory(pProcesses);
    }

    UniqueHandle hProcess;
    if (lsassPID) hProcess.reset(OpenProcess(PROCESS_QUERY_INFORMATION, FALSE, lsassPID));
    if (!hProcess && winlogonPID) hProcess.reset(OpenProcess(PROCESS_QUERY_INFORMATION, FALSE, winlogonPID));
    if (!hProcess) return false;

    HANDLE hToken = nullptr;
    if (!OpenProcessToken(hProcess.get(), TOKEN_DUPLICATE, &hToken)) return false;

    bool result = DuplicateTokenEx(hToken, MAXIMUM_ALLOWED, nullptr, SecurityIdentification, TokenPrimary, &tokenHandle);
    CloseHandle(hToken);
    return result;
}

static bool OpenServiceProcessToken(LPCWSTR serviceName, HANDLE& tokenHandle) {
    tokenHandle = nullptr;
    SC_HANDLE scm = OpenSCManagerW(nullptr, nullptr, SC_MANAGER_CONNECT);
    if (!scm) return false;
    UniqueServiceHandle scmGuard(scm);

    SC_HANDLE svc = OpenServiceW(scm, serviceName, SERVICE_QUERY_STATUS);
    if (!svc) return false;
    UniqueServiceHandle svcGuard(svc);

    SERVICE_STATUS_PROCESS status;
    DWORD needed;
    if (!QueryServiceStatusEx(svc, SC_STATUS_PROCESS_INFO, (LPBYTE)&status, sizeof(status), &needed))
        return false;
    if (status.dwCurrentState != SERVICE_RUNNING) return false;

    UniqueHandle hProcess(OpenProcess(PROCESS_QUERY_INFORMATION, FALSE, status.dwProcessId));
    if (!hProcess) return false;

    return OpenProcessToken(hProcess.get(), MAXIMUM_ALLOWED, &tokenHandle);
}

static bool AdjustTokenAllPrivileges(HANDLE hToken, DWORD attributes) {
    DWORD size = 0;
    GetTokenInformation(hToken, TokenPrivileges, nullptr, 0, &size);
    if (GetLastError() != ERROR_INSUFFICIENT_BUFFER) return false;

    std::vector<BYTE> buffer(size);
    if (!GetTokenInformation(hToken, TokenPrivileges, buffer.data(), size, &size)) return false;

    PTOKEN_PRIVILEGES pTp = (PTOKEN_PRIVILEGES)buffer.data();
    for (DWORD i = 0; i < pTp->PrivilegeCount; ++i)
        pTp->Privileges[i].Attributes = attributes;

    return AdjustTokenPrivileges(hToken, FALSE, pTp, size, nullptr, nullptr) && GetLastError() == ERROR_SUCCESS;
}

// ---------- 核心函数 ----------
static bool SimpleCreateProcess(
    int level,
    bool privileged,
    LPCWSTR commandLine,
    LPCWSTR workingDirectory,
    PROCESS_INFORMATION& pi)
{
    ZeroMemory(&pi, sizeof(PROCESS_INFORMATION));

    UniqueHandle currentToken, impersonatedToken, systemToken;
    UniqueHandle impersonatedSystem, trustedToken, targetToken;
    UniqueHandle envBlock;

    HANDLE hToken = nullptr;
    if (!OpenProcessToken(GetCurrentProcess(), MAXIMUM_ALLOWED, &hToken))
        return false;
    currentToken.reset(hToken);

    hToken = nullptr;
    if (!DuplicateTokenEx(currentToken.get(), MAXIMUM_ALLOWED, nullptr, SecurityImpersonation, TokenImpersonation, &hToken))
        return false;
    impersonatedToken.reset(hToken);

    LUID luid;
    if (!LookupPrivilegeValueW(nullptr, SE_DEBUG_NAME, &luid))
        return false;
    TOKEN_PRIVILEGES tp = { 1, { luid, SE_PRIVILEGE_ENABLED } };
    if (!AdjustTokenPrivileges(impersonatedToken.get(), FALSE, &tp, sizeof(tp), nullptr, nullptr) || GetLastError() != ERROR_SUCCESS)
        return false;

    if (!SetThreadToken(nullptr, impersonatedToken.get()))
        return false;

    hToken = nullptr;
    if (!CreateSystemToken(hToken))
        return false;
    systemToken.reset(hToken);

    hToken = nullptr;
    if (!DuplicateTokenEx(systemToken.get(), MAXIMUM_ALLOWED, nullptr, SecurityImpersonation, TokenImpersonation, &hToken))
        return false;
    impersonatedSystem.reset(hToken);

    if (!AdjustTokenAllPrivileges(impersonatedSystem.get(), SE_PRIVILEGE_ENABLED))
        return false;

    if (!SetThreadToken(nullptr, impersonatedSystem.get()))
        return false;

    hToken = nullptr;
    if (level == 0) {
        if (!DuplicateTokenEx(currentToken.get(), MAXIMUM_ALLOWED, nullptr, SecurityIdentification, TokenPrimary, &hToken))
            return false;
    }
    else if (level == 1) {
        if (!DuplicateTokenEx(systemToken.get(), MAXIMUM_ALLOWED, nullptr, SecurityIdentification, TokenPrimary, &hToken))
            return false;
    }
    else if (level == 2) {
        hToken = nullptr;
        if (!OpenServiceProcessToken(L"TrustedInstaller", hToken))
            return false;
        trustedToken.reset(hToken);

        hToken = nullptr;
        if (!DuplicateTokenEx(trustedToken.get(), MAXIMUM_ALLOWED, nullptr, SecurityIdentification, TokenPrimary, &hToken))
            return false;
    }
    else {
        SetLastError(ERROR_INVALID_PARAMETER);
        return false;
    }
    targetToken.reset(hToken);

    DWORD sessionId = GetActiveSessionID();
    if (!SetTokenInformation(targetToken.get(), TokenSessionId, &sessionId, sizeof(sessionId)))
        return false;

    if (privileged) {
        if (!AdjustTokenAllPrivileges(targetToken.get(), SE_PRIVILEGE_ENABLED))
            return false;
    }

    LPVOID pEnv = nullptr;
    if (!CreateEnvironmentBlock(&pEnv, currentToken.get(), TRUE))
        return false;
    envBlock.reset(pEnv);

    STARTUPINFOW si = { sizeof(si) };
    if (!CreateProcessAsUserW(targetToken.get(), nullptr, (LPWSTR)commandLine, nullptr, nullptr, TRUE,
        CREATE_UNICODE_ENVIRONMENT, envBlock.get(), workingDirectory, &si, &pi))
        return false;

    SetThreadToken(nullptr, nullptr);
    return true;
}

// ---------- 导出函数 ----------
extern "C" MINSUDO_API HRESULT MinSudoRun(
    LPCWSTR commandLine,
    MINSUDO_LEVEL level,
    BOOL privileged,
    LPCWSTR workingDirectory,
    MINSUDO_RESULT* result)
{
    if (!result) return E_POINTER;
    ZeroMemory(result, sizeof(MINSUDO_RESULT));

    if (!commandLine || wcslen(commandLine) == 0)
        commandLine = L"cmd.exe";

    std::wstring workDir;
    if (workingDirectory && wcslen(workingDirectory) > 0)
        workDir = workingDirectory;
    else
        workDir = GetWorkingDirectory();
    if (!workDir.empty() && workDir.back() == L'\\')
        workDir.pop_back();

    PROCESS_INFORMATION pi = { 0 };
    BOOL ok = SimpleCreateProcess((int)level, privileged != FALSE, commandLine, workDir.c_str(), pi);

    if (ok) {
        WaitForSingleObject(pi.hProcess, INFINITE);
        DWORD exitCode = 0;
        GetExitCodeProcess(pi.hProcess, &exitCode);
        CloseHandle(pi.hThread);
        CloseHandle(pi.hProcess);

        result->success = TRUE;
        result->exitCode = exitCode;
        result->errorCode = ERROR_SUCCESS;
        wcscpy_s(result->errorMessage, 256, L"Success");
        return S_OK;
    }
    else {
        DWORD err = GetLastError();
        result->success = FALSE;
        result->exitCode = 0;
        result->errorCode = err;
        FormatMessageW(FORMAT_MESSAGE_FROM_SYSTEM, NULL, err, 0,
            result->errorMessage, 256, NULL);
        return HRESULT_FROM_WIN32(err);
    }
}

// ---------- DLL 入口 ----------
BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
    return TRUE;
}