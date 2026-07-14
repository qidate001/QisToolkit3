#!/usr/bin/env python3
"""
齐之防御命令行交互工具
提供完整的命令行界面与QisDefense服务交互
"""

import os
import sys
import time
import argparse
import cmd
import shutil
from typing import List, Optional
from datetime import datetime

# 导入库
from pipe_client import QiPipeClient, LockMode, PipeClientError


class QiCLI(cmd.Cmd):
    """齐之防御交互式命令行"""
    
    intro = """
╔══════════════════════════════════════════════════════════════╗
║                  齐之防御 命令行管理工具                      ║
║                                                              ║
║  输入 help 查看命令列表，help <命令> 查看详细帮助           ║
║  输入 quit 或 Ctrl+C 退出                                   ║
╚══════════════════════════════════════════════════════════════╝
"""
    prompt = "Qi> "
    
    def __init__(self):
        super().__init__()
        self.client = QiPipeClient(timeout_ms=5000, auto_connect=False)
        self.connected = False
        self._check_service()
    
    def _check_service(self):
        """检查服务状态"""
        if QiPipeClient.is_service_running():
            try:
                self.client.connect()
                self.connected = True
                print("[✓] 已连接到齐之防御服务")
            except Exception as e:
                print(f"[✗] 连接失败: {e}")
        else:
            print("[!] 齐之防御服务未运行")
            response = input("是否启动服务？(y/n): ").strip().lower()
            if response == 'y':
                if QiPipeClient.start_service():
                    print("[✓] 服务启动成功，正在连接...")
                    try:
                        self.client.connect()
                        self.connected = True
                        print("[✓] 已连接到齐之防御服务")
                    except Exception as e:
                        print(f"[✗] 连接失败: {e}")
                else:
                    print("[✗] 服务启动失败，请手动启动")
    
    def _ensure_connected(self) -> bool:
        """确保已连接"""
        if not self.connected:
            print("[!] 未连接到服务，请检查服务是否运行")
            return False
        return True
    
    def _format_result(self, success: bool, operation: str, target: str = "") -> str:
        """格式化操作结果"""
        icon = "✓" if success else "✗"
        status = "成功" if success else "失败"
        msg = f"[{icon}] {operation}"
        if target:
            msg += f" {target}"
        msg += f" {status}"
        return msg
    
    # ========== 命令实现 ==========
    
    def do_ping(self, arg):
        """
        测试连接延迟
        用法: ping
        """
        if not self._ensure_connected():
            return
        
        success, delay = self.client.ping()
        if success:
            print(f"[✓] PONG! 延迟: {delay:.2f}ms")
        else:
            print("[✗] PING 失败")
    
    def do_status(self, arg):
        """
        查看服务状态
        用法: status
        """
        if not self._ensure_connected():
            return
        
        try:
            status = self.client.get_service_status()
            print("服务状态信息:")
            print(f"  运行中: {'是' if status.get('running') else '否'}")
            print(f"  进程ID: {status.get('pid', 'N/A')}")
            print(f"  运行时间: {status.get('uptime', 'N/A')}")
            print(f"  版本: {status.get('version', 'N/A')}")
        except Exception as e:
            print(f"[✗] 获取状态失败: {e}")
    
    def do_lock(self, arg):
        """
        锁定文件
        用法: lock <文件路径> [模式]
        模式: exclusive(独占), shared(共享), readonly(只读), writeonly(只写)
        默认: exclusive
        """
        if not self._ensure_connected():
            return
        
        args = arg.strip().split()
        if len(args) < 1:
            print("[!] 用法: lock <文件路径> [模式]")
            return
        
        file_path = args[0]
        mode_str = args[1] if len(args) > 1 else "exclusive"
        
        mode_map = {
            'exclusive': LockMode.EXCLUSIVE,
            'shared': LockMode.SHARED,
            'readonly': LockMode.READ_ONLY,
            'writeonly': LockMode.WRITE_ONLY,
        }
        
        mode = mode_map.get(mode_str.lower(), LockMode.EXCLUSIVE)
        
        if not os.path.exists(file_path):
            print(f"[!] 文件不存在: {file_path}")
            return
        
        try:
            success = self.client.lock_file(file_path, mode)
            print(self._format_result(success, "锁定文件", file_path))
        except Exception as e:
            print(f"[✗] 锁定失败: {e}")
    
    def do_unlock(self, arg):
        """
        解锁文件
        用法: unlock <文件路径>
        """
        if not self._ensure_connected():
            return
        
        file_path = arg.strip()
        if not file_path:
            print("[!] 用法: unlock <文件路径>")
            return
        
        try:
            success = self.client.unlock_file(file_path)
            print(self._format_result(success, "解锁文件", file_path))
        except Exception as e:
            print(f"[✗] 解锁失败: {e}")
    
    def do_check(self, arg):
        """
        检查文件锁定状态
        用法: check <文件路径>
        """
        if not self._ensure_connected():
            return
        
        file_path = arg.strip()
        if not file_path:
            print("[!] 用法: check <文件路径>")
            return
        
        try:
            locked = self.client.check_status(file_path)
            status = "已锁定" if locked else "未锁定"
            print(f"[✓] {file_path}: {status}")
        except Exception as e:
            print(f"[✗] 检查失败: {e}")
    
    def do_delete(self, arg):
        """
        删除文件（需要管理员权限）
        用法: delete <文件路径>
        """
        if not self._ensure_connected():
            return
        
        file_path = arg.strip()
        if not file_path:
            print("[!] 用法: delete <文件路径>")
            return
        
        if not os.path.exists(file_path):
            print(f"[!] 文件不存在: {file_path}")
            return
        
        confirm = input(f"确定要删除 {file_path} 吗？(y/n): ").strip().lower()
        if confirm != 'y':
            print("[!] 操作已取消")
            return
        
        try:
            success = self.client.delete_file(file_path)
            print(self._format_result(success, "删除文件", file_path))
        except Exception as e:
            print(f"[✗] 删除失败: {e}")
    
    def do_critical_add(self, arg):
        """
        添加关键进程
        用法: critical_add <进程ID>
        """
        if not self._ensure_connected():
            return
        
        pid_str = arg.strip()
        if not pid_str or not pid_str.isdigit():
            print("[!] 用法: critical_add <进程ID>")
            return
        
        pid = int(pid_str)
        try:
            success = self.client.add_critical_process(pid)
            print(self._format_result(success, "添加关键进程", f"PID:{pid}"))
        except Exception as e:
            print(f"[✗] 操作失败: {e}")
    
    def do_critical_remove(self, arg):
        """
        移除关键进程
        用法: critical_remove <进程ID>
        """
        if not self._ensure_connected():
            return
        
        pid_str = arg.strip()
        if not pid_str or not pid_str.isdigit():
            print("[!] 用法: critical_remove <进程ID>")
            return
        
        pid = int(pid_str)
        try:
            success = self.client.remove_critical_process(pid)
            print(self._format_result(success, "移除关键进程", f"PID:{pid}"))
        except Exception as e:
            print(f"[✗] 操作失败: {e}")
    
    def do_critical_list(self, arg):
        """
        列出所有关键进程
        用法: critical_list
        """
        if not self._ensure_connected():
            return
        
        try:
            pids = self.client.list_critical_processes()
            if pids:
                print("关键进程列表:")
                for pid in pids:
                    info = self.client.get_process_info(pid)
                    name = info.get('name', 'Unknown')
                    print(f"  PID: {pid} ({name})")
            else:
                print("[!] 没有关键进程")
        except Exception as e:
            print(f"[✗] 获取列表失败: {e}")
    
    def do_critical_check(self, arg):
        """
        检查进程是否为关键进程
        用法: critical_check <进程ID>
        """
        if not self._ensure_connected():
            return
        
        pid_str = arg.strip()
        if not pid_str or not pid_str.isdigit():
            print("[!] 用法: critical_check <进程ID>")
            return
        
        pid = int(pid_str)
        try:
            is_critical = self.client.is_critical_process(pid)
            status = "是" if is_critical else "否"
            print(f"[✓] PID {pid} 是否为关键进程: {status}")
        except Exception as e:
            print(f"[✗] 检查失败: {e}")
    
    def do_kill(self, arg):
        """
        终止进程
        用法: kill <进程ID> [--force]
        """
        if not self._ensure_connected():
            return
        
        args = arg.strip().split()
        if not args or not args[0].isdigit():
            print("[!] 用法: kill <进程ID> [--force]")
            return
        
        pid = int(args[0])
        force = '--force' in args
        
        confirm = input(f"确定要终止进程 {pid} 吗？(y/n): ").strip().lower()
        if confirm != 'y':
            print("[!] 操作已取消")
            return
        
        try:
            success = self.client.kill_process(pid, force)
            print(self._format_result(success, "终止进程", f"PID:{pid}"))
        except Exception as e:
            print(f"[✗] 操作失败: {e}")
    
    def do_jobkill(self, arg):
        """
        使用作业对象终止进程
        用法: jobkill <进程ID>
        """
        if not self._ensure_connected():
            return
        
        pid_str = arg.strip()
        if not pid_str or not pid_str.isdigit():
            print("[!] 用法: jobkill <进程ID>")
            return
        
        pid = int(pid_str)
        confirm = input(f"确定要终止进程 {pid} 吗？(y/n): ").strip().lower()
        if confirm != 'y':
            print("[!] 操作已取消")
            return
        
        try:
            success = self.client.job_kill_process(pid)
            print(self._format_result(success, "作业终止进程", f"PID:{pid}"))
        except Exception as e:
            print(f"[✗] 操作失败: {e}")
    
    def do_info(self, arg):
        """
        获取进程信息
        用法: info <进程ID>
        """
        if not self._ensure_connected():
            return
        
        pid_str = arg.strip()
        if not pid_str or not pid_str.isdigit():
            print("[!] 用法: info <进程ID>")
            return
        
        pid = int(pid_str)
        try:
            info = self.client.get_process_info(pid)
            if info:
                print(f"进程信息 (PID: {pid}):")
                print(f"  名称: {info.get('name', 'N/A')}")
                print(f"  路径: {info.get('path', 'N/A')}")
                print(f"  关键进程: {'是' if info.get('critical') else '否'}")
                print(f"  受保护: {'是' if info.get('protected') else '否'}")
                print(f"  状态: {info.get('status', 'N/A')}")
            else:
                print(f"[!] 未找到进程: {pid}")
        except Exception as e:
            print(f"[✗] 获取信息失败: {e}")
    
    def do_fileinfo(self, arg):
        """
        获取文件信息
        用法: fileinfo <文件路径>
        """
        if not self._ensure_connected():
            return
        
        file_path = arg.strip()
        if not file_path:
            print("[!] 用法: fileinfo <文件路径>")
            return
        
        try:
            info = self.client.get_file_info(file_path)
            if info and info.get('exists'):
                print(f"文件信息: {file_path}")
                print(f"  大小: {info.get('size', 0)} 字节")
                print(f"  创建时间: {info.get('created', 'N/A')}")
                print(f"  修改时间: {info.get('modified', 'N/A')}")
                print(f"  访问时间: {info.get('accessed', 'N/A')}")
            else:
                print(f"[!] 文件不存在或无法获取信息: {file_path}")
        except Exception as e:
            print(f"[✗] 获取信息失败: {e}")
    
    def do_batch_lock(self, arg):
        """
        批量锁定文件
        用法: batch_lock <文件1> <文件2> ... <文件N> [--mode exclusive]
        """
        if not self._ensure_connected():
            return
        
        args = arg.strip().split()
        if len(args) < 1:
            print("[!] 用法: batch_lock <文件1> <文件2> ... [--mode exclusive]")
            return
        
        mode = LockMode.EXCLUSIVE
        files = []
        i = 0
        while i < len(args):
            if args[i] == '--mode' and i + 1 < len(args):
                mode_map = {
                    'exclusive': LockMode.EXCLUSIVE,
                    'shared': LockMode.SHARED,
                    'readonly': LockMode.READ_ONLY,
                    'writeonly': LockMode.WRITE_ONLY,
                }
                mode = mode_map.get(args[i + 1].lower(), LockMode.EXCLUSIVE)
                i += 2
            else:
                files.append(args[i])
                i += 1
        
        if not files:
            print("[!] 请指定要锁定的文件")
            return
        
        print(f"正在锁定 {len(files)} 个文件...")
        results = self.client.lock_files(files, mode)
        
        success_count = sum(1 for v in results.values() if v)
        for path, success in results.items():
            print(self._format_result(success, "", path))
        
        print(f"\n总计: {success_count}/{len(files)} 成功")
    
    def do_batch_unlock(self, arg):
        """
        批量解锁文件
        用法: batch_unlock <文件1> <文件2> ... <文件N>
        """
        if not self._ensure_connected():
            return
        
        files = arg.strip().split()
        if not files:
            print("[!] 用法: batch_unlock <文件1> <文件2> ... <文件N>")
            return
        
        print(f"正在解锁 {len(files)} 个文件...")
        results = self.client.unlock_files(files)
        
        success_count = sum(1 for v in results.values() if v)
        for path, success in results.items():
            print(self._format_result(success, "", path))
        
        print(f"\n总计: {success_count}/{len(files)} 成功")
    
    def do_batch_delete(self, arg):
        """
        批量删除文件
        用法: batch_delete <文件1> <文件2> ... <文件N>
        """
        if not self._ensure_connected():
            return
        
        files = arg.strip().split()
        if not files:
            print("[!] 用法: batch_delete <文件1> <文件2> ... <文件N>")
            return
        
        print("将要删除以下文件:")
        for f in files:
            print(f"  {f}")
        
        confirm = input("确定要删除这些文件吗？(y/n): ").strip().lower()
        if confirm != 'y':
            print("[!] 操作已取消")
            return
        
        print(f"正在删除 {len(files)} 个文件...")
        results = self.client.delete_files(files)
        
        success_count = sum(1 for v in results.values() if v)
        for path, success in results.items():
            print(self._format_result(success, "", path))
        
        print(f"\n总计: {success_count}/{len(files)} 成功")
    
    def do_clear(self, arg):
        """清屏"""
        os.system('cls' if os.name == 'nt' else 'clear')
    
    def do_quit(self, arg):
        """退出程序"""
        print("再见！")
        self.client.disconnect()
        return True
    
    def do_help(self, arg):
        """显示帮助信息"""
        if arg:
            cmd.Cmd.do_help(self, arg)
        else:
            print("""
╔══════════════════════════════════════════════════════════════╗
║                     可用命令列表                             ║
╠══════════════════════════════════════════════════════════════╣
║  ping              - 测试连接延迟                            ║
║  status            - 查看服务状态                            ║
║  lock              - 锁定文件                                ║
║  unlock            - 解锁文件                                ║
║  check             - 检查文件锁定状态                        ║
║  delete            - 删除文件                                ║
║  fileinfo          - 获取文件信息                            ║
║  critical_add      - 添加关键进程                            ║
║  critical_remove   - 移除关键进程                            ║
║  critical_list     - 列出关键进程                            ║
║  critical_check    - 检查关键进程状态                        ║
║  kill              - 终止进程                                ║
║  jobkill           - 作业终止进程                            ║
║  info              - 获取进程信息                            ║
║  batch_lock        - 批量锁定文件                            ║
║  batch_unlock      - 批量解锁文件                            ║
║  batch_delete      - 批量删除文件                            ║
║  clear             - 清屏                                    ║
║  help              - 显示帮助                                ║
║  quit              - 退出程序                                ║
╚══════════════════════════════════════════════════════════════╝

输入 help <命令> 查看详细用法
""")
    
    def do_EOF(self, arg):
        """Ctrl+D退出"""
        print()
        return True


def main():
    """命令行入口"""
    parser = argparse.ArgumentParser(
        description="齐之防御命令行管理工具",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
示例:
  python qis_cli.py                # 进入交互式模式
  python qis_cli.py --ping         # 测试连接
  python qis_cli.py --status       # 查看服务状态
  python qis_cli.py --lock file.txt # 锁定文件
  python qis_cli.py --unlock file.txt # 解锁文件
        """
    )
    
    parser.add_argument('--ping', action='store_true', help='测试连接')
    parser.add_argument('--status', action='store_true', help='查看服务状态')
    parser.add_argument('--lock', metavar='FILE', help='锁定文件')
    parser.add_argument('--unlock', metavar='FILE', help='解锁文件')
    parser.add_argument('--check', metavar='FILE', help='检查文件锁定状态')
    parser.add_argument('--delete', metavar='FILE', help='删除文件')
    parser.add_argument('--critical-add', metavar='PID', help='添加关键进程')
    parser.add_argument('--critical-remove', metavar='PID', help='移除关键进程')
    parser.add_argument('--critical-list', action='store_true', help='列出关键进程')
    parser.add_argument('--kill', metavar='PID', help='终止进程')
    parser.add_argument('--info', metavar='PID', help='获取进程信息')
    parser.add_argument('--interactive', '-i', action='store_true', help='进入交互模式')
    
    args = parser.parse_args()
    
    # 如果没有任何参数，进入交互模式
    if len(sys.argv) == 1:
        try:
            cli = QiCLI()
            cli.cmdloop()
        except KeyboardInterrupt:
            print("\n退出")
        return
    
    # 单次命令模式
    try:
        client = QiPipeClient()
        
        if args.ping:
            success, delay = client.ping()
            print(f"PONG! 延迟: {delay:.2f}ms" if success else "PING 失败")
        
        elif args.status:
            status = client.get_service_status()
            print(f"服务状态: {'运行中' if status.get('running') else '已停止'}")
            if status.get('running'):
                print(f"PID: {status.get('pid')}")
                print(f"运行时间: {status.get('uptime')}")
                print(f"版本: {status.get('version')}")
        
        elif args.lock:
            success = client.lock_file(args.lock)
            print(f"锁定 {'成功' if success else '失败'}: {args.lock}")
        
        elif args.unlock:
            success = client.unlock_file(args.unlock)
            print(f"解锁 {'成功' if success else '失败'}: {args.unlock}")
        
        elif args.check:
            locked = client.check_status(args.check)
            print(f"{args.check}: {'已锁定' if locked else '未锁定'}")
        
        elif args.delete:
            success = client.delete_file(args.delete)
            print(f"删除 {'成功' if success else '失败'}: {args.delete}")
        
        elif args.critical_add:
            success = client.add_critical_process(int(args.critical_add))
            print(f"添加关键进程 {'成功' if success else '失败'}: PID {args.critical_add}")
        
        elif args.critical_remove:
            success = client.remove_critical_process(int(args.critical_remove))
            print(f"移除关键进程 {'成功' if success else '失败'}: PID {args.critical_remove}")
        
        elif args.critical_list:
            pids = client.list_critical_processes()
            if pids:
                print("关键进程:")
                for pid in pids:
                    print(f"  {pid}")
            else:
                print("没有关键进程")
        
        elif args.kill:
            success = client.kill_process(int(args.kill))
            print(f"终止进程 {'成功' if success else '失败'}: PID {args.kill}")
        
        elif args.info:
            info = client.get_process_info(int(args.info))
            if info:
                print(f"进程信息 (PID: {args.info}):")
                print(f"  名称: {info.get('name')}")
                print(f"  路径: {info.get('path')}")
                print(f"  关键进程: {info.get('critical')}")
            else:
                print(f"未找到进程: {args.info}")
        
        elif args.interactive:
            cli = QiCLI()
            cli.cmdloop()
        
        else:
            parser.print_help()
    
    except KeyboardInterrupt:
        print("\n退出")
    except Exception as e:
        print(f"错误: {e}")
        sys.exit(1)


if __name__ == "__main__":
    main()