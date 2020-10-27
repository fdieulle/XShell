using System.Windows;
using XShell.Demo.Wpf;

namespace XShell.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly MyXShellModule _module = new MyXShellModule();

        #region Overrides of Application

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _module.Run();
            _module.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _module.Dispose();
        }

        #endregion
    }
}
