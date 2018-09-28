using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChannelDemoTool
{
    class DependentFiles
    {
        // 当前命名空间名称
        //private static string NAMESPACE = GetNamespace(Assembly.GetEntryAssembly());
        private static string NAMESPACE = "ChannelDemoTool";

        //string ToolUrl = "https://git.oschina.net/joymeng/" + NAMESPACE + "/raw/master/MD5.txt";
        public static string ToolUrl = "https://raw.githubusercontent.com/joymeng/" + NAMESPACE + "/master/MD5.txt";

        /// <summary>
        /// 获取Assembly所在的命名空间名称
        /// </summary>
        private static string GetNamespace(Assembly asssembly)
        {
            string Namespace = "";
            Type[] types = asssembly.GetTypes();
            if (types != null && types.Length > 0)
            {
                Namespace = types[0].Namespace;
            }
            return Namespace;
        }

        /// <summary>
        /// 获取当前运行路径
        /// </summary>
        public static string curDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 检测目录是否存在，若不存在则创建
        /// </summary>
        public static void checkDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 保存Byte数组为文件
        /// </summary>
        public static void SaveFile(Byte[] array, string path, bool repalce = false)
        {
            if (repalce && System.IO.File.Exists(path)) System.IO.File.Delete(path);    // 若目标文件存在，则替换
            if (!System.IO.File.Exists(path))
            {
                // 创建输出流
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);

                //将byte数组写入文件中
                fs.Write(array, 0, array.Length);
                fs.Close();
            }
        }

        /// <summary>
        /// 为arg添加引号
        /// </summary>
        private static string AddQuotation(string arg)
        {
            if (arg.EndsWith("\\") && !arg.EndsWith("\\\\")) arg += "\\";
            arg = "\"" + arg + "\"";

            return arg;
        }

        // "%~dp0Update.exe" "[CONFIG]https://git.oschina.net/joymeng/ChannelConfig/raw/master/MD5.txt" "E:\tmp2\Update_Files\\" "渠道计费包\0000001\\"
        /// <summary>
        /// 调用Update.exe，更新以perfix为前缀的配置文件
        /// </summary>
        public static void updateFiles(string url, string perfix, bool RESTART = false)
        {
            string update_EXE = curDir() + "Update.exe";
            if (!File.Exists(update_EXE)) SaveFile(ChannelDemoTool.Properties.Resources.Update, update_EXE, false);
            else if (!updateIsRunning()) SaveFile(ChannelDemoTool.Properties.Resources.Update, update_EXE, true);   // 更新Update.exe

            string path = curDir();
            url = AddQuotation("[CONFIG]" + url);
            path = AddQuotation(path);
            perfix = AddQuotation(perfix);
            update_EXE = AddQuotation(update_EXE);

            // 调用更新插件执行软件更新逻辑
            String arg = url + " " + path + " " + perfix;
            if (RESTART) arg += " " + "RESTART";
            System.Diagnostics.Process.Start(update_EXE, arg);
        }

        /// <summary>
        /// 应用自更新
        /// </summary>
        public static void AutoUpdate()
        {
            string perfix = NAMESPACE + ".exe";
            updateFiles(ToolUrl, perfix, true);
        }

        /// <summary>
        /// 打开本地路径
        /// </summary>
        public static void openChannelDir(string channelDir)
        {
            string localDir = curDir() + channelDir.Replace("/", "\\");
            checkDir(localDir);

            if (Directory.Exists(localDir))
            {
                System.Diagnostics.Process.Start("explorer.exe", "/e, " + localDir);
            }
        }

        // 判断当前是有更新任务正在执行
        public static bool updateIsRunning()
        {
            // 获取Update.exe后台进程
            System.Diagnostics.Process[] processes1 = System.Diagnostics.Process.GetProcessesByName("Update");

            // 无更新进程Update.exe，表示更新完成
            return (processes1 != null && processes1.Length != 0);
        }
    }
}

