using System;
using System.IO;
using XShell.Services;

namespace XShell
{
    public abstract class AbstractXShellModule<TIocContainer, TMainWindow, TScreen, TPopup, TBaseView> : IDisposable
        where TScreen : IScreenHost
        where TMainWindow : IMainWindow<TScreen>
        where TBaseView : class
        where TPopup : IPopupScreenHost
    {
        public string Name { get; }

        protected TIocContainer Container { get; }

        public TMainWindow MainWindow { get; private set; }

        protected AbstractXShellModule(string name, TIocContainer container)
        {
            Name = name;
            Container = container;
        }

        protected abstract void RegisterService<T>(T instance);
        protected abstract T ResolveService<T>();

        public void Run(bool withUi = true)
        {
            RunServices();

            if (withUi)
                RunUi();

            Initialize(Container);

            if (withUi)
                LoadWorkspace(ResolveService<IWorkspaceManager>());
        }

        private void RunServices()
        {
            var persistenceService = new PersistenceService
            {
                Folder = Path.Combine(GetAppDataFolder(), "Data")
            };

            RegisterService<IPersistenceService>(persistenceService);

            SetupServices(Container);
        }

        protected abstract void SetupServices(TIocContainer container);

        #region UI

        private void RunUi()
        {
            MainWindow = CreateMainWindow($@"{Name} {GetRunningVersion()}");
            MainWindow.Terminating += OnMainWindowTerminated;

            RegisterService(CreateMenuManager(MainWindow));
            var screenManager = CreateScreenManager(
                MainWindow,
                ResolveService<IMenuManager>(),
                ResolveService<IPersistenceService>());
            RegisterService<IScreenContainer>(screenManager);
            RegisterService<IScreenManager>(screenManager);

            RegisterService(CreateViewBox());
            RegisterService<IWorkspaceManager>(new WorkspaceManager<TBaseView, TScreen, TPopup>(MainWindow, screenManager, ResolveService<IViewBox>())
            {
                Folder = Path.Combine(GetAppDataFolder(), "Workspace")
            });
            
            RegisterService(CreateUiDispatcher());
            RegisterService<IBackgroundTaskManager>(new BackgroundTaskManager(ResolveService<IUiDispatcher>()));

            SetupUi();
            SetupScreens(ResolveService<IScreenContainer>());
        }

        protected abstract TMainWindow CreateMainWindow(string title);

        protected abstract IMenuManager CreateMenuManager(TMainWindow mainWindow);

        protected abstract AbstractScreenManager<TBaseView, TScreen, TPopup> CreateScreenManager(TMainWindow mainWindow,
            IMenuManager menuManager, IPersistenceService persistenceService);

        protected abstract IViewBox CreateViewBox();

        protected abstract IUiDispatcher CreateUiDispatcher();

        protected virtual void SetupUi() { }

        protected abstract void SetupScreens(IScreenContainer container);

        private void OnMainWindowTerminated() => SaveWorkspace(ResolveService<IWorkspaceManager>());

        #region Workspace

        protected virtual void LoadWorkspace(IWorkspaceManager workspaceManager) => workspaceManager.Load();

        protected virtual void SaveWorkspace(IWorkspaceManager workspaceManager) => workspaceManager.Save();

        #endregion

        #endregion

        protected abstract void Initialize(TIocContainer container);
        
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
            if(Container is IDisposable disposable)
                disposable.Dispose();
        }

        #endregion
    }
}