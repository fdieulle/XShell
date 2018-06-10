using System;
using DryIoc;
using XShell.Demo.Wpf.Screens.Popup;
using XShell.Demo.Wpf.Screens.Screen;
using XShell.Demo.Wpf.Screens.ScreenWithoutInterface;
using XShell.Demo.Wpf.Screens.SimpleScreen;
using XShell.Demo.Wpf.Services.Service;
using XShell.Services;
using XShell.Wpf.Services.Shell;

namespace XShell.Demo.Wpf
{
    public class XShellModule
    {
        private readonly IContainer container;

        public XShellModule(MainWindow window)
        {
            container = new Container();

            var persistenceService = new PersistenceService();
            var menuManager = new DefaultMenuManager(window.MainMenu);
            var screenManager = new WindowAvalonDockScreenManager(
                window, window.MainPane,
                (p1, p2) => container.Register(p1, p2, setup: Setup.With(allowDisposableTransient: true)),
                container.Resolve, menuManager, persistenceService);

            container.RegisterInstance<IPersistenceService>(persistenceService);
            container.RegisterInstance<IMenuManager>(menuManager);
            container.RegisterInstance<IScreenContainer>(screenManager);
            container.RegisterInstance<IScreenManager>(screenManager);
            container.Register<IUiDispatcher, UiDispatcher>(Reuse.Singleton);
            container.Register<IBackgroundTaskManager, BackgroundTaskManager>(Reuse.Singleton);
            container.RegisterInstance(new StatusBarManager(window.BackgroundWorkerView, container.Resolve<IBackgroundTaskManager>()));

            RegisterServices(container);
            RegisterScreens(screenManager);

            window.Closed += OnClosed;
        }

        private static void RegisterServices(IContainer container)
        {
            container.RegisterInstance<IService>(new ServiceImpl());
        }

        private static void RegisterScreens(IScreenContainer container)
        {
            container.Register<SimpleScreenView>();
            container.Register<ScreenWithoutInterfaceView, ScreenWithoutInterfaceViewModel>();
            container.Register<IMyScreen, MyScreenView, MyScreenViewModel>();
            container.Register<IMyPopup, MyPopupView, MyPopupViewModel>();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            container.Dispose();
        }
    }
}
