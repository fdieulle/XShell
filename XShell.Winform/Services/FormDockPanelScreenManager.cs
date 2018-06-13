using System;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XShell.Services;
using XShell.Winform.Controls;

namespace XShell.Winform.Services
{
    public class FormDockPanelScreenManager : AbstractScreenManager<Control>
    {
        private readonly Form _mainForm;
        private readonly DockPanel _mainPanel;

        public FormDockPanelScreenManager(
            Form mainForm,
            DockPanel mainPanel,
            Action<Type, Type> register, Func<Type, object> resolve, 
            IMenuManager menuManager = null, 
            IPersistenceService persistenceService = null) 
            : base(register, resolve, menuManager, persistenceService)
        {
            _mainForm = mainForm;
            _mainPanel = mainPanel;
        }

        #region Overrides of AbstractScreenManager<TabPage>

        protected override IScreenHost CreateScreen(Control view)
        {
            var host = new XDockContent();
            view.Dock = DockStyle.Fill;
            view.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            host.Controls.Add(view);
            host.Show(_mainPanel, DockState.Document);
            return host;
        }

        protected override IScreenHost CreatePopup(Control view, PopupAttribute attribute)
        {
            var host = new XForm();
            view.Dock = DockStyle.Fill;
            view.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            host.Controls.Add(view);

            if(attribute != null)
                SetupPopup(host, attribute);

            host.Show(_mainForm);
            return host;
        }

        private void SetupPopup(XForm popup, PopupAttribute attribute)
        {
            switch (attribute.StartupLocation)
            {
                case StartupLocation.Manual:
                    popup.StartPosition = FormStartPosition.Manual;
                    popup.Top = (int)attribute.Top;
                    popup.Left = (int)attribute.Left;
                    break;
                case StartupLocation.MousePosition:
                    popup.StartPosition = FormStartPosition.Manual;
                    var position = Cursor.Position;
                    popup.Top = position.Y;
                    popup.Left = position.X;
                    break;
                case StartupLocation.CenterScreen:
                    popup.StartPosition = FormStartPosition.CenterScreen;
                    break;
                case StartupLocation.CenterOwner:
                    popup.StartPosition = FormStartPosition.CenterParent;
                    break;
            }

            if (attribute.Width > 0)
                popup.Width = (int)attribute.Width;
            if (attribute.Height > 0)
                popup.Height = (int)attribute.Height;

            switch (attribute.ResizeMode)
            {
                case ResizeMode.AutoSize:
                    popup.AutoSize = true;
                    break;
                case ResizeMode.CanResizeWithGrip:
                    popup.SizeGripStyle = SizeGripStyle.Show;
                    break;
                case ResizeMode.CanResize:
                case ResizeMode.NoResize:
                    popup.SizeGripStyle = SizeGripStyle.Hide;
                    break;
            }

            if (!string.IsNullOrEmpty(attribute.Icon))
                popup.Icon = new Icon(attribute.Icon);

            popup.TopMost = attribute.TopMost;
        }

        protected override void OnException(string message, Exception e)
        {
            throw e;
        }

        #endregion
    }
}
