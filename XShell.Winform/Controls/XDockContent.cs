using System;
using WeifenLuo.WinFormsUI.Docking;
using XShell.Services;

namespace XShell.Winform.Controls
{
    public class XDockContent : DockContent, IScreenHost
    {
        private readonly string _persistenceId = Guid.NewGuid().ToString("N");

        #region Implementation of IScreenHost

        public event Action<IScreenHost> ScreenClosed;

        public string Title { get => Text; set => Text = value; }

        public string GetPersistenceId() => _persistenceId;

        #endregion

        #region Overrides of DockContent

        protected override string GetPersistString() => _persistenceId;

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ScreenClosed?.Invoke(this);
        }
    }
}
