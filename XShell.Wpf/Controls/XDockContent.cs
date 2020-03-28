using System;
using XShell.Services;
using Xceed.Wpf.AvalonDock.Layout;

namespace XShell.Wpf.Controls
{
    public class XDockContent : LayoutDocument, IScreenHost
    {
        #region Implementation of IScreenHost

        public event Action<IScreenHost> ScreenClosed;

        public void BringToFront() => IsActive = true;

        #endregion

        protected override void OnClosed()
        {
            base.OnClosed();

            ScreenClosed?.Invoke(this);
        }
    }
}
