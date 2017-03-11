using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XShell.Demo.Winform.Controls;
using XShell.Services;

namespace XShell.Demo.Winform.Services.Shell
{
    public class FormDockPanelScreenManager : AbstractScreenManager<Control>
    {
        private readonly Form mainForm;
        private readonly DockPanel mainPanel;

        public FormDockPanelScreenManager(
            Form mainForm,
            DockPanel mainPanel,
            Action<Type, Type> register, Func<Type, object> resolve, 
            IMenuManager menuManager = null, 
            IPersistenceService persistenceService = null) 
            : base(register, resolve, menuManager, persistenceService)
        {
            this.mainForm = mainForm;
            this.mainPanel = mainPanel;
        }

        #region Overrides of AbstractScreenManager<TabPage>

        protected override IScreenHost CreateScreen(Control view)
        {
            var host = new XDockContent();
            view.Dock = DockStyle.Fill;
            view.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            host.Controls.Add(view);
            host.Show(this.mainPanel, DockState.Document);
            return host;
        }

        protected override IScreenHost CreatePopup(Control view)
        {
            var host = new XForm();
            view.Dock = DockStyle.Fill;
            view.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            host.Controls.Add(view);
            host.Show(this.mainForm);
            return host;
        }

        protected override void OnException(string message, Exception e)
        {
            throw e;
        }

        #endregion
    }
}
