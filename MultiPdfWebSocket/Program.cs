using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiPdfWebSocket
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            // 防止程序多次启动
            Process[] processes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (processes.Length > 1)
            {
                MessageBox.Show("MultiPdfWebSocket服务已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Thread.Sleep(1000);
                Environment.Exit(1);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
