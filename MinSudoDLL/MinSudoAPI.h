#pragma once

// 确保 Windows 类型已定义
#ifndef _WINDOWS_
#include <windows.h>
#endif

// 导出宏
#ifdef MINSUDO_EXPORTS
#define MINSUDO_API __declspec(dllexport)
#else
#define MINSUDO_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

    typedef enum _MINSUDO_LEVEL {
        MINSUDO_LEVEL_STANDARD = 0,
        MINSUDO_LEVEL_SYSTEM = 1,
        MINSUDO_LEVEL_TRUSTEDINSTALLER = 2
    } MINSUDO_LEVEL;

    typedef struct _MINSUDO_RESULT {
        BOOL success;
        DWORD exitCode;
        DWORD errorCode;
        WCHAR errorMessage[256];
    } MINSUDO_RESULT;

    // 导出函数声明
    MINSUDO_API HRESULT MinSudoRun(
        LPCWSTR commandLine,
        MINSUDO_LEVEL level,
        BOOL privileged,
        LPCWSTR workingDirectory,
        MINSUDO_RESULT* result
    );

#ifdef __cplusplus
}
#endif