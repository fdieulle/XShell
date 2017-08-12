using System;
using System.Windows.Forms;
using XShell.Services;

namespace XShell.Winform.Controls
{
    public class XForm : Form, IScreenHost
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
