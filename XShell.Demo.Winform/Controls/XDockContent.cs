using System;
using WeifenLuo.WinFormsUI.Docking;
using XShell.Services;

namespace XShell.Demo.Winform.Controls
{
    public class XDockContent : DockContent, IScreenHost
    {
        #region Implementation of IScreenHost

        public event Action<IScreenHost> ScreenClosed;

        public string Title { get { return Text; } set { Text = value; } }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            var handler = ScreenClosed;
            if (handler != null) handler(this);
        }
    }
}
