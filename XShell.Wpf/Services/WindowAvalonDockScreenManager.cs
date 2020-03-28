using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Xceed.Wpf.AvalonDock.Layout;
using XShell.Services;
using XShell.Wpf.Controls;

namespace XShell.Wpf.Services
{
    public class WindowAvalonDockScreenManager : AbstractScreenManager<FrameworkElement>
    {
        private readonly Window _mainWindow;
        private readonly LayoutDocumentPane _dockingManager;

        public WindowAvalonDockScreenManager(
            Window mainWindow, LayoutDocumentPane dockingManager,
            Action<Type, Type> register, Func<Type, object> resolve,
            IMenuManager menuManager = null, IPersistenceService persistenceService = null)
            : base(register, resolve, menuManager, persistenceService)
        {
            _mainWindow = mainWindow;
            _dockingManager = dockingManager;
        }

        #region Overrides of AbstractScreenManager<FrameworkElement>

        protected override IScreenHost CreateScreen(FrameworkElement view)
        {
            var doc = new XDockContent { Content = view };
            _dockingManager.Children.Add(doc);
            doc.IsActive = true;
            return doc;
        }

        protected override IScreenHost CreatePopup(FrameworkElement view, PopupAttribute attribute)
        {
            var popup = new XWindow { Owner = _mainWindow, Content = view };

            if (attribute != null)
                SetupPopup(popup, attribute);

            popup.Show();
            return popup;
        }

        private void SetupPopup(Window popup, PopupAttribute attribute)
        {
            switch (attribute.StartupLocation)
            {
                case StartupLocation.Manual:
                    popup.WindowStartupLocation = WindowStartupLocation.Manual;
                    popup.Top = attribute.Top;
                    popup.Left = attribute.Left;
                    break;
                case StartupLocation.MousePosition:
                    popup.WindowStartupLocation = WindowStartupLocation.Manual;
                    var position = Mouse.GetPosition(Application.Current.MainWindow);
                    if (Application.Current.MainWindow != null)
                        position = Application.Current.MainWindow.PointToScreen(position);
                    popup.Top = position.Y;
                    popup.Left = position.X;
                    break;
                case StartupLocation.CenterScreen:
                    popup.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    break;
                case StartupLocation.CenterOwner:
                    popup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    break;
            }

            if (attribute.Width > 0)
                popup.Width = attribute.Width;
            if (attribute.Height > 0)
                popup.Height = attribute.Height;

            switch (attribute.ResizeMode)
            {
                case ResizeMode.AutoSize:
                    popup.SizeToContent = SizeToContent.WidthAndHeight;
                    break;
                case ResizeMode.NoResize:
                    popup.ResizeMode = System.Windows.ResizeMode.NoResize;
                    break;
                case ResizeMode.CanResizeWithGrip:
                    popup.ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
                    break;
                case ResizeMode.CanResize:
                    popup.ResizeMode = System.Windows.ResizeMode.CanResize;
                    break;
            }

            if (!string.IsNullOrEmpty(attribute.Icon))
                popup.Icon = new BitmapImage(new Uri(attribute.Icon));

            popup.Topmost = attribute.TopMost;
        }

        protected override void OnException(string message, Exception e)
        {
            throw e;
        }

        #endregion
    }
}
