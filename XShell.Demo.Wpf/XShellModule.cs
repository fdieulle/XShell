using System;
using DryIoc;
using XShell.Demo.Wpf.Screens.Popup;
using XShell.Demo.Wpf.Screens.Screen;
using XShell.Demo.Wpf.Screens.ScreenWithoutInterface;
using XShell.Demo.Wpf.Screens.SimpleScreen;
using XShell.Demo.Wpf.Services.Service;
using XShell.Services;
using XShell.Wpf.Services;
using XShell.Wpf.Services.Shell;

namespace XShell.Demo.Wpf
{
    public class XShellModule
    {
        private readonly IContainer _container;

        public XShellModule(MainWindow window)
        {
            _container = new Container();

            var persistenceService = new PersistenceService();
            var menuManager = new DefaultMenuManager(window.MainMenu);
            var screenManager = new WindowAvalonDockScreenManager(
                window, window.MainPane,
                (p1, p2) => _container.Register(p1, p2, setup: Setup.With(allowDisposableTransient: true)),
                _container.Resolve, menuManager, persistenceService);

            _container.RegisterInstance<IPersistenceService>(persistenceService);
            _container.RegisterInstance<IMenuManager>(menuManager);
            _container.RegisterInstance<IScreenContainer>(screenManager);
            _container.RegisterInstance<IScreenManager>(screenManager);
            _container.Register<IViewBox, ViewBox>();
            _container.Register<IUiDispatcher, UiDispatcher>(Reuse.Singleton);
            _container.Register<IBackgroundTaskManager, BackgroundTaskManager>(Reuse.Singleton);
            _container.RegisterInstance(new StatusBarManager(window.BackgroundWorkerView, _container.Resolve<IBackgroundTaskManager>()));

            RegisterServices(_container);
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
            _container.Dispose();
        }
    }
}
