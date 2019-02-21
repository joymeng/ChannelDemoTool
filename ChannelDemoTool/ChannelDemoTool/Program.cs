using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChannelDemoTool
{
    // 示例：Program.cs
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args = null)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SciTools.RunningExceptionTool.Run(call);                     // 捕获运行异常
            //SciTools.RunningExceptionTool.Run(call, SendErrors);       // 捕获运行异常，并发送异常信息
            //SciTools.RunningExceptionTool.Run(call, SendErrors, args); // 捕获运行异常，并发送异常信息, Main方法有参数
        }

        /// <summary>
        /// 应用程序，入口逻辑
        /// </summary>
        public static void call(string[] args = null)
        {
            DependentFiles.AutoUpdate();
            Application.Run(new MainForm());
        }

        /// <summary>
        // 自定义异常信息处理逻辑
        /// </summary>
        public static void SendErrors(string errorMsg)
        {
            // 自定义异常信息处理逻辑
        }
    }

    //static class Program
    //{
    //    /// <summary>
    //    /// 应用程序的主入口点。
    //    /// </summary>
    //    [STAThread]
    //    static void Main()
    //    {
    //        Application.EnableVisualStyles();
    //        Application.SetCompatibleTextRenderingDefault(false);

    //        DependentFiles.AutoUpdate();

    //        Application.Run(new MainForm());
    //    }
    //}
}
