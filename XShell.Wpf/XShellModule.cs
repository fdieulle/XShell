using System;
using System.IO;
using System.Windows;
using DryIoc;
using XShell.Services;
using XShell.Wpf.Controls;
using XShell.Wpf.Services;

namespace XShell.Wpf
{
    public abstract class XShellModule : IDisposable
    {
        public string Name { get; }

        protected IContainer Container { get; }

        public MainWindow MainWindow { get; private set; }

        protected XShellModule(string name, IContainer container = null)
        {
            Name = name;
            Container = container ?? new Container();
        }

        public void Run(bool withUi = true)
        {
            RunServices();

            if (withUi)
                RunUi();

            Initialize(Container);
        }

        private void RunServices()
        {
            var persistenceService = new PersistenceService
            {
                Folder = Path.Combine(GetAppDataFolder(), "Data")
            };

            Container.UseInstance<IPersistenceService>(persistenceService);

            SetupServices(Container);
        }

        protected abstract void SetupServices(IContainer container);

        private void RunUi()
        {
            MainWindow = new MainWindow
            {
                Title = $@"{Name} {GetRunningVersion()}"
            };

            var menuManager = new DefaultMenuManager(MainWindow.Menu);
            var screenManager = new WindowAvalonDockScreenManager(
                MainWindow, MainWindow.Docker,
                (p1, p2) => Container.Register(p1, p2, setup: Setup.With(allowDisposableTransient: true)),
                Container.Resolve, menuManager, Container.Resolve<IPersistenceService>());

            Container.Register<IViewBox, ViewBox>(Reuse.Singleton);
            var workspaceManager = new WorkspaceManager<FrameworkElement, XDockContent, XWindow>(MainWindow, screenManager, Container.Resolve<IViewBox>())
            {
                Folder = Path.Combine(GetAppDataFolder(), "Workspace")
            };

            Container.UseInstance<IWorkspaceManager>(workspaceManager);
            Container.UseInstance<IMenuManager>(menuManager);
            Container.UseInstance<IScreenContainer>(screenManager);
            Container.UseInstance<IScreenManager>(screenManager);
            Container.Register<IUiDispatcher, UiDispatcher>(Reuse.Singleton);
            Container.Register<IBackgroundTaskManager, BackgroundTaskManager>(Reuse.Singleton);
            Container.UseInstance(new StatusBarManager(MainWindow.BackgroundWorker, Container.Resolve<IBackgroundTaskManager>()));

            SetupScreens(screenManager);
        }

        protected abstract void SetupScreens(IScreenContainer container);

        protected abstract void Initialize(IContainer container);

        private string GetAppDataFolder()
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Name ?? "XShell");

        private string GetRunningVersion()
        {
#if DEBUG
            return "Debug";
#else
            try
            {
                return ApplicationDeployment.IsNetworkDeployed ? 
                    ApplicationDeployment.CurrentDeployment.CurrentVersion : 
                    Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
#endif
        }

        #region IDisposable

        public void Dispose()
        {
            Container?.Dispose();
        }

        #endregion
    }
}
