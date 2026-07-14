# 齐之防御管道客户端示例

这是一个与齐之防御（QisDefense）服务通过命名管道通信的示例程序。

> **注意**: 此工具仅为示例，展示Windows命名管道通信和CLI开发的技巧。
> 实际使用需要安装齐之防御服务。

## 功能特性

- ✅ 命名管道通信
- ✅ 文件锁定/解锁
- ✅ 进程保护管理
- ✅ 交互式命令行界面
- ✅ 批量操作支持

## 系统要求

- Windows 7/8/10/11
- Python 3.7+
- pywin32 库
- 齐之防御服务

## 安装

```bash
pip install pywin32
```

## 运行交互式程序
```bash
python qis_cli.py
```

## 单次命令模式
```bash
# 测试连接
python qis_cli.py --ping

# 查看服务状态
python qis_cli.py --status

# 锁定文件
python qis_cli.py --lock "C:\test.txt"

# 解锁文件
python qis_cli.py --unlock "C:\test.txt"

# 检查文件状态
python qis_cli.py --check "C:\test.txt"

# 删除文件
python qis_cli.py --delete "C:\test.txt"

# 添加关键进程
python qis_cli.py --critical-add 1234

# 列出关键进程
python qis_cli.py --critical-list

# 终止进程
python qis_cli.py --kill 1234
```

## 交互式命令示例
```text
Qi> ping
[✓] PONG! 延迟: 15.23ms

Qi> lock C:\test.txt exclusive
[✓] 锁定文件 C:\test.txt 成功

Qi> check C:\test.txt
[✓] C:\test.txt: 已锁定

Qi> critical_add 1234
[✓] 添加关键进程 PID:1234 成功

Qi> critical_list
关键进程列表:
  PID: 1234 (notepad.exe)

Qi> quit
再见！
```