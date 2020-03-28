using System;
using System.Windows.Forms;

namespace XShell.Winform.Services
{
    public class UiDispatcher : IUiDispatcher
    {
        private readonly Control _control;

        public UiDispatcher(Control control) => _control = control;

        public void Dispatch(Action action) => _control.BeginInvoke(action);
    }
}
