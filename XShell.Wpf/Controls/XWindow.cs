using System;
using System.Windows;
using XShell.Services;

namespace XShell.Wpf.Controls
{
    public class XWindow : Window, IScreenHost
    {
        #region Implementation of IScreenHost

        public event Action<IScreenHost> ScreenClosed;

        public void BringToFront() => Activate();

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ScreenClosed?.Invoke(this);
        }
    }
}
