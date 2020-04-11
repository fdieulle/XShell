using System.Windows;
using DryIoc;
using XShell.Services;
using XShell.Wpf.Controls;
using XShell.Wpf.Services;

namespace XShell.Wpf
{
    public abstract class XShellModule : AbstractXShellModule<IContainer, MainWindow, XDockContent, XWindow, FrameworkElement>
    {
        protected XShellModule(string name, IContainer container = null)
            : base(name, container ?? new Container()) { }

        #region Overrides of XShellModule<IContainer,MainForm,XDockContent,XForm,Control>

        protected override void RegisterService<T>(T instance)
            => Container.UseInstance(instance);

        protected override T ResolveService<T>()
            => Container.Resolve<T>();

        protected override MainWindow CreateMainWindow(string title)
            => new MainWindow { Title = title };

        protected override IMenuManager CreateMenuManager(MainWindow mainWindow)
            => new DefaultMenuManager(mainWindow.Menu);

        protected override AbstractScreenManager<FrameworkElement, XDockContent, XWindow> CreateScreenManager(
            MainWindow mainWindow, IMenuManager menuManager, IPersistenceService persistenceService)
            => new WindowAvalonDockScreenManager(
                mainWindow,
                mainWindow.Docker,
                (p1, p2) => Container.Register(p1, p2, setup: Setup.With(allowDisposableTransient: true)),
                Container.Resolve,
                menuManager,
                persistenceService);

        protected override IViewBox CreateViewBox()
            => new ViewBox();

        protected override IUiDispatcher CreateUiDispatcher()
            => new UiDispatcher();

        protected override void SetupUi()
        {
            base.SetupUi();
            Container.UseInstance(new StatusBarManager(MainWindow.BackgroundWorker, Container.Resolve<IBackgroundTaskManager>()));
        }

        #endregion
    }
}
