using System.Windows;
using DryIoc;
using XShell.Demo.Wpf.Screens.Popup;
using XShell.Demo.Wpf.Screens.Screen;
using XShell.Demo.Wpf.Screens.ScreenWithoutInterface;
using XShell.Demo.Wpf.Screens.SimpleScreen;
using XShell.Demo.Wpf.Services.Service;
using XShell.Demo.Wpf.Services.Shell;
using XShell.Services;

namespace XShell.Demo.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IContainer container;

        public MainWindow()
        {
            InitializeComponent();

            container = new Container();

            var persistenceService = new PersistenceService();
            var menuManager = new DefaultMenuManager(MainMenu);
            var screenManager = new WindowAvalonDockScreenManager(
                this, MainPane,
                (p1, p2) => container.Register(p1, p2, setup: Setup.With(allowDisposableTransient: true)),
                container.Resolve, menuManager, persistenceService);

            container.RegisterInstance<IPersistenceService>(persistenceService);
            container.RegisterInstance<IMenuManager>(menuManager);
            container.RegisterInstance<IScreenContainer>(screenManager);
            container.RegisterInstance<IScreenManager>(screenManager);

            RegisterServices(container);
            RegisterScreens(screenManager);
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

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);

            container.Dispose();
        }
    }
}
