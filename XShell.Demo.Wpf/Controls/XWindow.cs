using System;
using System.Windows;
using XShell.Services;

namespace XShell.Demo.Wpf.Controls
{
    public class XWindow : Window, IScreenHost
    {
        #region Implementation of IScreenHost

        public event Action<IScreenHost> ScreenClosed;
        
        public void BringToFront()
        {
            Activate();

        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            var handler = ScreenClosed;
            if (handler != null)
                handler(this);
        }
    }
}
