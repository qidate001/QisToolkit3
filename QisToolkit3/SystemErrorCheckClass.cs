using QisToolkit3.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Qi;
using static QisToolkit3.Forms.AdvancedModificationSystemTools;

namespace QisToolkit3
{
    public interface ISystemCheck
    {
        string Name { get; }
        string Description { get; }
        bool Check();
        bool Repair();
    }


    // Ctf 未运行
    public class CtfMonitorCheck : ISystemCheck
    {
        public string Name => "CTF 加载程序未运行";
        public string Description => "CTF加载程序（ctfmon.exe）未运行会导致输入法消失错误";

        public bool Check()
        {
            return !IsRuning("ctfmon");
        }

        public bool Repair()
        {
            // 实现修复逻辑，例如启动 ctfmon.exe
            try
            {
                Process.Start("ctfmon.exe");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }


    // SystemProfile目录结构缺失
    public class SystemProfileCheck : ISystemCheck
    {
        public string Name => "SystemProfile目录结构缺失";
        public string Description => 
            "systemprofile目录内容结构缺失可能导致一些System账户的前端操作报错\n" +
            "对于普通用户而言，完全无影响\nC:\\WINDOWS\\system32\\config\\systemprofile";

        public bool Check()
        {
            return 
                !(
                    Directory.Exists(@"C:\WINDOWS\system32\config\systemprofile\Desktop") &&
                    Directory.Exists(@"C:\WINDOWS\system32\config\systemprofile\Documents") &&
                    Directory.Exists(@"C:\WINDOWS\system32\config\systemprofile\Pictures") &&
                    Directory.Exists(@"C:\WINDOWS\system32\config\systemprofile\Downloads") &&
                    Directory.Exists(@"C:\WINDOWS\system32\config\systemprofile\Music") 
                );
        }

        public bool Repair()
        {
            try
            {
                Directory.CreateDirectory(@"C:\WINDOWS\system32\config\systemprofile\Desktop");
                Directory.CreateDirectory(@"C:\WINDOWS\system32\config\systemprofile\Documents");
                Directory.CreateDirectory(@"C:\WINDOWS\system32\config\systemprofile\Pictures");
                Directory.CreateDirectory(@"C:\WINDOWS\system32\config\systemprofile\Downloads");
                Directory.CreateDirectory(@"C:\WINDOWS\system32\config\systemprofile\Music");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }


    // SystemProfile目录结构缺失
    public class SystemRegPoliciesDisable : ISystemCheck
    {
        string DisableList = GetList();

        public string Name => "系统功能被禁用";
        public string Description =>
            "被功能禁用的功能：\n" + DisableList;

        public bool Check()
        {
            return !string.IsNullOrEmpty(DisableList);
        }

        public bool Repair()
        {
            try
            {
                // 禁用所有策略
                Ext_SetRegPoliciesExplorer(false, 0);
                Ext_SetRegPoliciesExplorer(false, 1);

                // 重新获取列表
                DisableList = GetList();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetList()
        {
            string List1 = Ext_GetRegPoliciesExplorer(0); 
            string List2 = Ext_GetRegPoliciesExplorer(1);
            if (!string.IsNullOrEmpty(List1)) List1 = "当前用户策略：" + List1;
            if (!string.IsNullOrEmpty(List2)) List2 = "所有用户策略：" + List2;

            string List3 = "";
            if (!string.IsNullOrEmpty(List1) && !string.IsNullOrEmpty(List2))
                List3 = List1 + "\n\n" + List2;
            else
                List3 = List1 + List2;

            return List3;
        }
    }
}
