using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using DryIoc;
using XShell.Services;
using XShell.Winform.Controls;
using XShell.Winform.Services;
using Container = DryIoc.Container;
using IContainer = DryIoc.IContainer;

namespace XShell.Winform
{
    public abstract class XShellModule : IDisposable
    {
        public string Name { get; }

        protected IContainer Container { get; }

        public MainForm MainWindow { get; private set; }

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

            if (withUi)
                LoadWorkspace(Container.Resolve<IWorkspaceManager>());
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
            MainWindow = new MainForm
            {
                Text = $@"{Name} {GetRunningVersion()}"
            };
            MainWindow.Terminating += OnMainWindowTerminating;
            
            var menuManager = new ToolStripMenuManager(MainWindow.MainMenuStrip);
            var screenManager = new FormDockPanelScreenManager(MainWindow, MainWindow.MainDockPanel,
                (p1, p2) => Container.Register(p1, p2, setup: Setup.With(allowDisposableTransient: true)), 
                Container.Resolve,
                menuManager, Container.Resolve<IPersistenceService>());

            Container.Register<IViewBox, ViewBox>(Reuse.Singleton);
            var workspaceManager = new WorkspaceManager<Control, XDockContent, XForm>(MainWindow, screenManager, Container.Resolve<IViewBox>())
                {
                    Folder = Path.Combine(GetAppDataFolder(), "Workspace")
                };

            Container.UseInstance<IWorkspaceManager>(workspaceManager);
            Container.UseInstance<IMenuManager>(menuManager);
            Container.UseInstance<IScreenContainer>(screenManager);
            Container.UseInstance<IScreenManager>(screenManager);
            Container.UseInstance<IUiDispatcher>(new UiDispatcher(MainWindow));
            Container.Register<IBackgroundTaskManager, BackgroundTaskManager>(Reuse.Singleton);
            Container.UseInstance(new StatusBarManager(MainWindow.StatusProgressBar, MainWindow.StatusProgressLabel, Container.Resolve<IBackgroundTaskManager>()));

            SetupScreens(screenManager);
        }

        protected abstract void SetupScreens(IScreenContainer container);

        protected abstract void Initialize(IContainer container);

        protected virtual void LoadWorkspace(IWorkspaceManager workspaceManager) => workspaceManager.Load();

        protected virtual void SaveWorkspace(IWorkspaceManager workspaceManager) => workspaceManager.Save();

        private void OnMainWindowTerminating()
        {
            SaveWorkspace(Container.Resolve<IWorkspaceManager>());
        }

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
