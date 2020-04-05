using System;
using System.Windows.Forms;
using XShell.Services;

namespace XShell.Winform.Controls
{
    public class XForm : Form, IPopupScreenHost
    {
        private readonly string _persistenceId = Guid.NewGuid().ToString("N");

        #region Implementation of IScreenHost

        public event Action<IScreenHost> ScreenClosed;

        public string Title { get => Text; set => Text = value; }

        public string GetPersistenceId() => _persistenceId;

        public RectangleSettings GetPositionAndSize()
        {
            return new RectangleSettings
            {
                Top = Top,
                Left = Left,
                Width = Width,
                Height = Height
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
