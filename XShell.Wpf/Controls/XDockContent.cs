using System;
using XShell.Services;
using Xceed.Wpf.AvalonDock.Layout;

namespace XShell.Wpf.Controls
{
    public class XDockContent : LayoutDocument, IScreenHost
    {
        private readonly string _persistenceId = Guid.NewGuid().ToString("N");

        #region Implementation of IScreenHost

        public event Action<IScreenHost> ScreenClosed;

        public void BringToFront() => IsActive = true;

        public string GetPersistenceId() => ContentId ?? (ContentId = _persistenceId);

        #endregion

        protected override void OnClosed()
        {
            base.OnClosed();

            ScreenClosed?.Invoke(this);
        }
    }
}
