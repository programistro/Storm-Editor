using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Storm_IDE
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Global_catch);
            Application.Run(new Form1(args));
        }
        private static void Global_catch(object sender, ThreadExceptionEventArgs e)
        {
            Trace.WriteLine(e.Exception.ToString());
        }
    }
}
