using System;
using System.Windows.Forms;

namespace XShell.Demo.Winform
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var module = new MyXShellModule();
            module.Run();

            Application.Run(module.MainWindow);

            module.Dispose();
        }
    }
}
