"""
齐之防御管道客户端库
提供与QisDefense服务通信的完整功能
"""

import os
import sys
import time
import subprocess
from typing import Optional, Tuple, List, Dict
from enum import IntEnum


class LockMode(IntEnum):
    """文件锁定模式"""
    EXCLUSIVE = 1      # 独占锁
    SHARED = 2         # 共享锁
    READ_ONLY = 3      # 只读锁
    WRITE_ONLY = 4     # 只写锁


class PipeClientError(Exception):
    """管道客户端异常基类"""
    pass


class ServiceNotRunningError(PipeClientError):
    """服务未运行异常"""
    pass


class ConnectionTimeoutError(PipeClientError):
    """连接超时异常"""
    pass


class CommandError(PipeClientError):
    """命令执行错误"""
    pass


class QiPipeClient:
    """
    齐之防御管道客户端
    
    用于与QisDefense服务通过命名管道通信
    """
    
    PIPE_NAME = r"\\.\pipe\QisDefensePipe"
    DEFAULT_TIMEOUT = 5000  # 毫秒
    BUFFER_SIZE = 4096
    SERVICE_NAME = "QisDefense.exe"
    
    def __init__(self, timeout_ms: int = 5000, auto_connect: bool = True):
        """
        初始化客户端
        
        Args:
            timeout_ms: 默认超时时间（毫秒）
            auto_connect: 是否自动连接
        """
        self.timeout_ms = timeout_ms
        self._connected = False
        self._handle = None
        
        if auto_connect:
            self.connect()
    
    def connect(self, timeout_ms: Optional[int] = None) -> bool:
        """
        连接到命名管道
        
        Args:
            timeout_ms: 超时时间，默认使用实例设置
            
        Returns:
            是否连接成功
        """
        if self._connected and self._handle:
            return True
            
        timeout = timeout_ms or self.timeout_ms
        
        try:
            import win32file
            import win32pipe
            import pywintypes
            
            # 尝试连接
            start_time = time.time()
            last_error = None
            
            while time.time() - start_time < timeout / 1000.0:
                try:
                    self._handle = win32file.CreateFile(
                        self.PIPE_NAME,
                        win32file.GENERIC_READ | win32file.GENERIC_WRITE,
                        0,
                        None,
                        win32file.OPEN_EXISTING,
                        0,
                        None
                    )
                    
                    # 设置管道模式
                    try:
                        win32pipe.SetNamedPipeHandleState(
                            self._handle, 
                            win32pipe.PIPE_READMODE_MESSAGE, 
                            None, 
                            None
                        )
                    except:
                        pass
                    
                    self._connected = True
                    return True
                    
                except pywintypes.error as e:
                    last_error = e
                    if e.winerror == 2:  # 文件不存在，服务未启动
                        time.sleep(0.1)
                        continue
                    elif e.winerror == 231:  # 所有管道实例都在忙
                        time.sleep(0.1)
                        continue
                    else:
                        raise ConnectionTimeoutError(f"连接失败: {e}")
            
            raise ConnectionTimeoutError(f"连接超时 ({timeout}ms): {last_error}")
            
        except ImportError:
            return self._connect_ctypes(timeout)
    
    def _connect_ctypes(self, timeout_ms: int) -> bool:
        """使用ctypes连接"""
        import ctypes
        from ctypes import wintypes
        
        kernel32 = ctypes.WinDLL('kernel32', use_last_error=True)
        
        GENERIC_READ = 0x80000000
        GENERIC_WRITE = 0x40000000
        OPEN_EXISTING = 3
        
        start_time = time.time()
        
        while time.time() - start_time < timeout_ms / 1000.0:
            handle = kernel32.CreateFileW(
                self.PIPE_NAME,
                GENERIC_READ | GENERIC_WRITE,
                0,
                None,
                OPEN_EXISTING,
                0,
                None
            )
            
            if handle != -1:
                self._handle = handle
                self._connected = True
                return True
            
            time.sleep(0.1)
        
        raise ConnectionTimeoutError(f"连接超时 ({timeout_ms}ms)")
    
    def disconnect(self):
        """断开连接"""
        if self._handle:
            try:
                import win32file
                win32file.CloseHandle(self._handle)
            except:
                try:
                    import ctypes
                    kernel32 = ctypes.WinDLL('kernel32', use_last_error=True)
                    kernel32.CloseHandle(self._handle)
                except:
                    pass
            
            self._handle = None
            self._connected = False
    
    def send_command(self, command: str, timeout_ms: Optional[int] = None) -> str:
        """
        发送命令并接收响应
        
        Args:
            command: 命令字符串
            timeout_ms: 超时时间
            
        Returns:
            服务端响应
            
        Raises:
            ServiceNotRunningError: 服务未运行
            ConnectionTimeoutError: 连接超时
            CommandError: 命令执行失败
        """
        if not self._connected or not self._handle:
            self.connect(timeout_ms)
        
        timeout = timeout_ms or self.timeout_ms
        
        try:
            import win32file
            
            # 发送命令
            command_bytes = command.encode('utf-8')
            win32file.WriteFile(self._handle, command_bytes)
            
            # 接收响应
            result, data = win32file.ReadFile(self._handle, self.BUFFER_SIZE)
            
            if result == 0:
                response = data.decode('utf-8', errors='ignore')
                if response.startswith("ERROR|"):
                    raise CommandError(response[6:])
                return response
            else:
                raise CommandError(f"读取失败，错误码: {result}")
                
        except ImportError:
            return self._send_command_ctypes(command, timeout)
        except Exception as e:
            raise CommandError(str(e))
    
    def _send_command_ctypes(self, command: str, timeout_ms: int) -> str:
        """使用ctypes发送命令"""
        import ctypes
        from ctypes import wintypes
        
        kernel32 = ctypes.WinDLL('kernel32', use_last_error=True)
        
        # 确保连接
        if not self._connected or not self._handle:
            self.connect(timeout_ms)
        
        try:
            # 发送命令
            command_bytes = command.encode('utf-8')
            bytes_written = wintypes.DWORD()
            
            result = kernel32.WriteFile(
                self._handle,
                command_bytes,
                len(command_bytes),
                ctypes.byref(bytes_written),
                None
            )
            
            if not result:
                raise CommandError(f"写入失败: {ctypes.get_last_error()}")
            
            # 接收响应
            buffer = ctypes.create_string_buffer(self.BUFFER_SIZE)
            bytes_read = wintypes.DWORD()
            
            result = kernel32.ReadFile(
                self._handle,
                buffer,
                self.BUFFER_SIZE,
                ctypes.byref(bytes_read),
                None
            )
            
            if not result:
                raise CommandError(f"读取失败: {ctypes.get_last_error()}")
            
            response = buffer.raw[:bytes_read.value].decode('utf-8', errors='ignore')
            if response.startswith("ERROR|"):
                raise CommandError(response[6:])
            return response
            
        finally:
            pass
    
    def __enter__(self):
        """上下文管理器入口"""
        self.connect()
        return self
    
    def __exit__(self, exc_type, exc_val, exc_tb):
        """上下文管理器出口"""
        self.disconnect()
    
    # ========== 服务管理 ==========
    
    @classmethod
    def is_service_running(cls) -> bool:
        """检查齐之防御服务是否运行"""
        try:
            client = cls(auto_connect=False)
            client.connect(timeout_ms=1000)
            response = client.send_command("PING", timeout_ms=1000)
            client.disconnect()
            return response.startswith("PONG|")
        except:
            return False
    
    @classmethod
    def start_service(cls, service_path: Optional[str] = None) -> bool:
        """
        启动齐之防御服务
        
        Args:
            service_path: 服务文件路径，默认在当前目录查找
            
        Returns:
            是否启动成功
        """
        if cls.is_service_running():
            return True
        
        if not service_path:
            # 在当前目录和常见位置查找
            search_paths = [
                os.path.dirname(os.path.abspath(__file__)),
                os.getcwd(),
                os.path.join(os.environ.get('ProgramFiles', ''), 'QisToolkit'),
                os.path.join(os.environ.get('ProgramFiles(x86)', ''), 'QisToolkit'),
            ]
            
            for path in search_paths:
                exe_path = os.path.join(path, cls.SERVICE_NAME)
                if os.path.exists(exe_path):
                    service_path = exe_path
                    break
        
        if not service_path or not os.path.exists(service_path):
            return False
        
        try:
            subprocess.Popen([service_path], shell=True)
            # 等待服务启动
            time.sleep(2)
            return cls.is_service_running()
        except:
            return False
    
    @classmethod
    def ensure_service_running(cls, auto_start: bool = True, 
                               service_path: Optional[str] = None) -> bool:
        """
        确保服务正在运行
        
        Args:
            auto_start: 如果服务未运行，是否自动启动
            service_path: 服务文件路径
            
        Returns:
            服务是否可用
        """
        if cls.is_service_running():
            return True
        
        if not auto_start:
            return False
        
        return cls.start_service(service_path)
    
    # ========== 文件操作 ==========
    
    def lock_file(self, file_path: str, mode: LockMode = LockMode.EXCLUSIVE) -> bool:
        """锁定文件"""
        response = self.send_command(f"LOCK|{file_path}|{mode.value}")
        return response.startswith("OK|")
    
    def unlock_file(self, file_path: str) -> bool:
        """解锁文件"""
        response = self.send_command(f"UNLOCK|{file_path}")
        return response.startswith("OK|")
    
    def check_status(self, file_path: str) -> bool:
        """检查文件锁定状态"""
        response = self.send_command(f"STATUS|{file_path}")
        return response.startswith("LOCKED|true")
    
    def delete_file(self, file_path: str) -> bool:
        """删除文件（需要管理员权限）"""
        response = self.send_command(f"DELETE_FILE|{file_path}")
        return response.startswith("OK|true")
    
    def get_file_info(self, file_path: str) -> Dict:
        """获取文件详细信息"""
        response = self.send_command(f"FILE_INFO|{file_path}")
        if response.startswith("OK|"):
            # 解析返回数据
            parts = response[3:].split('|')
            if len(parts) >= 5:
                return {
                    'exists': parts[0] == 'true',
                    'size': int(parts[1]) if parts[1].isdigit() else 0,
                    'created': parts[2],
                    'modified': parts[3],
                    'accessed': parts[4],
                }
        return {}
    
    # ========== 进程操作 ==========
    
    def add_critical_process(self, process_id: int) -> bool:
        """添加关键进程"""
        response = self.send_command(f"ADD_CRITICAL|{process_id}")
        return response.startswith("OK|")
    
    def remove_critical_process(self, process_id: int) -> bool:
        """移除关键进程"""
        response = self.send_command(f"REMOVE_CRITICAL|{process_id}")
        return response.startswith("OK|")
    
    def is_critical_process(self, process_id: int) -> bool:
        """检查进程是否为关键进程"""
        response = self.send_command(f"CHECK_CRITICAL|{process_id}")
        return response.startswith("OK|true")
    
    def kill_process(self, process_id: int, force: bool = False) -> bool:
        """终止进程"""
        cmd = "KILL_PROCESS_FORCE" if force else "KILL_PROCESS"
        response = self.send_command(f"{cmd}|{process_id}")
        return response.startswith("OK|true")
    
    def job_kill_process(self, process_id: int) -> bool:
        """使用作业对象终止进程"""
        response = self.send_command(f"JOB_KILL_PROCESS|{process_id}")
        return response.startswith("OK|true")
    
    def get_process_info(self, process_id: int) -> Dict:
        """获取进程信息"""
        response = self.send_command(f"PROCESS_INFO|{process_id}")
        if response.startswith("OK|"):
            parts = response[3:].split('|')
            if len(parts) >= 6:
                return {
                    'pid': int(parts[0]) if parts[0].isdigit() else 0,
                    'name': parts[1],
                    'path': parts[2],
                    'critical': parts[3] == 'true',
                    'protected': parts[4] == 'true',
                    'status': parts[5],
                }
        return {}
    
    def list_critical_processes(self) -> List[int]:
        """列出所有关键进程"""
        response = self.send_command("LIST_CRITICAL")
        if response.startswith("OK|"):
            pids = response[3:].split('|')
            return [int(pid) for pid in pids if pid.isdigit()]
        return []
    
    # ========== 系统操作 ==========
    
    def get_service_status(self) -> Dict:
        """获取服务状态"""
        response = self.send_command("SERVICE_STATUS")
        if response.startswith("OK|"):
            parts = response[3:].split('|')
            if len(parts) >= 4:
                return {
                    'running': parts[0] == 'true',
                    'pid': int(parts[1]) if parts[1].isdigit() else 0,
                    'uptime': parts[2],
                    'version': parts[3],
                }
        return {}
    
    def ping(self) -> Tuple[bool, float]:
        """测试连接延迟"""
        start = time.time()
        try:
            response = self.send_command("PING", timeout_ms=1000)
            elapsed = (time.time() - start) * 1000
            return response.startswith("PONG|"), elapsed
        except:
            return False, -1
    
    # ========== 批量操作 ==========
    
    def lock_files(self, file_paths: List[str], mode: LockMode = LockMode.EXCLUSIVE) -> Dict[str, bool]:
        """批量锁定文件"""
        results = {}
        for path in file_paths:
            results[path] = self.lock_file(path, mode)
        return results
    
    def unlock_files(self, file_paths: List[str]) -> Dict[str, bool]:
        """批量解锁文件"""
        results = {}
        for path in file_paths:
            results[path] = self.unlock_file(path)
        return results
    
    def delete_files(self, file_paths: List[str]) -> Dict[str, bool]:
        """批量删除文件"""
        results = {}
        for path in file_paths:
            results[path] = self.delete_file(path)
        return results