using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ChannelDemoTool
{
    class Cmd
    {
        /// <summary>
        /// 创建包含cmd命令的.bat文件，并执行
        /// </summary>
        public static string Run_bat(string cmd)
        {
            String bat = createTmpBat(cmd);

            Process P = newProcess(bat);
            string outStr = P.StandardOutput.ReadToEnd();
            P.Close();

            if (File.Exists(bat)) File.Delete(bat);
            return outStr;
        }

        static bool singleBat = true;
        /// <summary>
        /// 创建指定命令的bat文件
        /// </summary>
        public static string createTmpBat(String cmd)
        {
            String filePath = AppDomain.CurrentDomain.BaseDirectory + (singleBat ? "CMD_FILE" : DateTime.Now.Ticks.ToString()) + ".bat";
            FileProcess.SaveProcess(cmd, filePath, Encoding.Default);

            return filePath;
        }

        /// <summary>
        /// 以后台进程的形式执行应用程序（d:\*.exe）
        /// </summary>
        public static Process newProcess(String exe)
        {
            Process P = new Process();
            P.StartInfo.CreateNoWindow = true;
            P.StartInfo.FileName = exe;
            P.StartInfo.UseShellExecute = false;
            P.StartInfo.RedirectStandardError = true;
            P.StartInfo.RedirectStandardInput = true;
            P.StartInfo.RedirectStandardOutput = true;
            //P.StartInfo.WorkingDirectory = @"C:\windows\system32";
            P.Start();
            return P;
        }
    }

    public class FileProcess
    {
        #region 文件读取与保存

        /// <summary>
        /// 获取文件中的数据串
        /// </summary>
        public static string fileToString(String filePath)
        {
            string str = "";

            //获取文件内容
            if (System.IO.File.Exists(filePath))
            {
                bool defaultEncoding = filePath.EndsWith(".txt");

                System.IO.StreamReader file1;

                file1 = new System.IO.StreamReader(filePath);                  //读取文件中的数据
                //if (defaultEncoding) file1 = new System.IO.StreamReader(filePath, Encoding.Default);//读取文件中的数据
                //else file1 = new System.IO.StreamReader(filePath);                  //读取文件中的数据

                str = file1.ReadToEnd();                                            //读取文件中的全部数据

                file1.Close();
                file1.Dispose();
            }
            return str;
        }

        /// <summary>
        /// 保存数据data到文件处理过程，返回值为保存的文件名
        /// </summary>
        public static String SaveProcess(String data, String filePath, Encoding encoding = null)
        {
            //不存在该文件时先创建
            System.IO.StreamWriter file1 = null;
            if (encoding == null) file1 = new System.IO.StreamWriter(filePath, false/*, System.Text.Encoding.UTF8*/);     //文件已覆盖方式添加内容
            else file1 = new System.IO.StreamWriter(filePath, false, Encoding.Default);     // 使用指定的格式进行保存

            file1.Write(data);                                                              //保存数据到文件

            file1.Close();                                                                  //关闭文件
            file1.Dispose();                                                                //释放对象

            return filePath;
        }

        /// <summary>
        /// 获取当前运行目录
        /// </summary>
        public static string CurDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        #endregion
    }
}
