using System;
using System.Windows;
using XShell.Demo.Wpf.Controls;
using XShell.Services;
using Xceed.Wpf.AvalonDock.Layout;

namespace XShell.Demo.Wpf.Services.Shell
{
    public class WindowAvalonDockScreenManager : AbstractScreenManager<FrameworkElement>
    {
        private readonly Window mainWindow;
        private readonly LayoutDocumentPane dockingManager;

        public WindowAvalonDockScreenManager(
            Window mainWindow, LayoutDocumentPane dockingManager,
            Action<Type, Type> register, Func<Type, object> resolve,
            IMenuManager menuManager = null, IPersistenceService persistenceService = null)
            : base(register, resolve, menuManager, persistenceService)
        {
            this.mainWindow = mainWindow;
            this.dockingManager = dockingManager;
        }

        #region Overrides of AbstractScreenManager<FrameworkElement>

        protected override IScreenHost CreateScreen(FrameworkElement view)
        {
            var doc = new XDockContent { Content = view };
            dockingManager.Children.Add(doc);
            doc.IsActive = true;
            return doc;
        }

        protected override IScreenHost CreatePopup(FrameworkElement view)
        {
            var popup = new XWindow { Owner = mainWindow, Content = view };
            popup.Show();
            return popup;
        }

        protected override void OnException(string message, Exception e)
        {
            throw e;
        }

        #endregion
    }
}
