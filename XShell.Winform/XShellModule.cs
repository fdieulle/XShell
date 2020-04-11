using System.Windows.Forms;
using DryIoc;
using XShell.Services;
using XShell.Winform.Controls;
using XShell.Winform.Services;

namespace XShell.Winform
{
    public abstract class XShellModule : AbstractXShellModule<IContainer, MainForm, XDockContent, XForm, Control>
    {
        protected XShellModule(string name, IContainer container = null) 
            : base(name, container ?? new Container()) { }

        #region Overrides of XShellModule<IContainer,MainForm,XDockContent,XForm,Control>

        protected override void RegisterService<T>(T instance) 
            => Container.UseInstance(instance);

        protected override T ResolveService<T>()
            => Container.Resolve<T>();

        protected override MainForm CreateMainWindow(string title) 
            => new MainForm { Text = title };

        protected override IMenuManager CreateMenuManager(MainForm mainWindow)
            => new ToolStripMenuManager(mainWindow.MainMenuStrip);

        protected override AbstractScreenManager<Control, XDockContent, XForm> CreateScreenManager(
            MainForm mainWindow, IMenuManager menuManager, IPersistenceService persistenceService) 
            => new FormDockPanelScreenManager(
                mainWindow,
                mainWindow.MainDockPanel,
                (p1, p2) => Container.Register(p1, p2, setup: Setup.With(allowDisposableTransient: true)),
                Container.Resolve,
                menuManager,
                persistenceService);

        protected override IViewBox CreateViewBox() 
            => new ViewBox();

        protected override IUiDispatcher CreateUiDispatcher() 
            => new UiDispatcher(MainWindow);

        protected override void SetupUi()
        {
            base.SetupUi();
            Container.UseInstance(new StatusBarManager(MainWindow.StatusProgressBar, MainWindow.StatusProgressLabel, Container.Resolve<IBackgroundTaskManager>()));
        }

        #endregion
    }
}
