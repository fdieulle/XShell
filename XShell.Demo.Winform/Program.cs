using System;
using System.Windows.Forms;
using DryIoc;
using XShell.Demo.Winform.Screens.Popup;
using XShell.Demo.Winform.Screens.Screen;
using XShell.Demo.Winform.Screens.ScreenWithoutInterface;
using XShell.Demo.Winform.Screens.SimpleScreen;
using XShell.Demo.Winform.Services.Service;
using XShell.Demo.Winform.Services.Shell;
using XShell.Services;

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

            var container = new Container();
            var mainForm = new MainForm();

            var persistenceService = new PersistenceService();
            var menuManager = new ToolStripMenuManager(mainForm.MainMenuStrip);
            var screenManager = new FormDockPanelScreenManager(mainForm, mainForm.MainDockPanel,
                (p1, p2) => container.Register(p1, p2, setup: Setup.With(allowDisposableTransient: true)), container.Resolve,
                menuManager, persistenceService);

            container.RegisterInstance<IPersistenceService>(persistenceService);
            container.RegisterInstance<IMenuManager>(menuManager);
            container.RegisterInstance<IScreenContainer>(screenManager);
            container.RegisterInstance<IScreenManager>(screenManager);

            RegisterServices(container);
            RegisterScreens(screenManager);

            Application.Run(mainForm);

            container.Dispose();
        }

        private static void RegisterServices(IContainer container)
        {
            container.RegisterInstance<IService>(new ServiceImpl());
        }

        private static void RegisterScreens(IScreenContainer container)
        {
            container.Register<SimpleScreenView>();
            container.Register<ScreenWithoutInterfaceView, ScreenWithoutInterfaceController>();
            container.Register<IMyScreen, MyScreenView, MyScreenController>();
            container.Register<IMyPopup, MyPopupView, MyPopupController>();
        }
    }
}
