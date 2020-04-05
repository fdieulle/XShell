using System;
using System.Windows;
using XShell.Services;

namespace XShell.Wpf.Controls
{
    public class XWindow : Window, IPopupScreenHost
    {
        private readonly string _persistenceId = Guid.NewGuid().ToString("N");

        #region Implementation of IScreenHost

        public event Action<IScreenHost> ScreenClosed;

        public void BringToFront() => Activate();

        public string GetPersistenceId() => _persistenceId;

        public RectangleSettings GetPositionAndSize()
        {
            return new RectangleSettings
            {
                Top = Top,
                Left = Left,
                Width = ActualWidth,
                Height = ActualHeight
            };
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ScreenClosed?.Invoke(this);
        }
    }
}
