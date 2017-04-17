using System;
using System.Windows.Forms;

namespace XShell.Demo.Winform.Services.Shell
{
    public class UiDispatcher : IUiDispatcher
    {
        private readonly Control control;

        public UiDispatcher(Control control)
        {
            this.control = control;
        }

        #region Implementation of IUiDispatcher

        public void Dispatch(Action action)
        {
            control.BeginInvoke(action);
        }

        #endregion
    }
}
