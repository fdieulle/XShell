using System;
using System.ComponentModel;
using System.Windows.Forms;
using XShell.Core;

namespace XShell.Winform.Binders
{
    public class ButtonBinder : IDisposable
    {
        private readonly Button button;
        private readonly IRelayCommand command;
        private readonly Func<object> getParameter;
        private readonly bool bindName;
        private readonly ToolTip toolTip;

        public ButtonBinder(Button button, IRelayCommand command, Func<object> getParameter = null, bool bindName = true, ToolTip toolTip = null)
        {
            this.button = button;
            this.command = command;
            this.getParameter = getParameter;
            this.bindName = bindName;
            this.toolTip = toolTip;

            this.button.Enabled = this.command.CanExecute(getParameter != null ? getParameter() : null);
            if (this.bindName)
                this.button.Text = this.command.Name;
            if (this.toolTip != null)
                this.toolTip.SetToolTip(this.button, this.command.Name);

            this.button.Click += OnButtonClick;
            this.command.CanExecuteChanged += OnCommandCanExecuteChanged;
            this.command.PropertyChanged += OnCommandPropertyChanged;
        }

        private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Core.Properties.NAME:
                    if (this.bindName)
                        this.button.Text = this.command.Name;
                    if (this.toolTip != null)
                        this.toolTip.SetToolTip(this.button, this.command.Name);
                    break;
            }
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            this.button.Enabled = this.command.CanExecute(getParameter != null ? getParameter() : null);
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            this.command.Execute(getParameter != null ? getParameter() : null);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.button.Click -= OnButtonClick;
            this.command.CanExecuteChanged -= OnCommandCanExecuteChanged;
            this.command.PropertyChanged -= OnCommandPropertyChanged;
        }

        #endregion
    }
}
