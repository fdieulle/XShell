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

        public ButtonBinder(Button button, IRelayCommand command, Func<object> getParameter = null)
        {
            this.button = button;
            this.command = command;
            this.getParameter = getParameter;

            this.button.Enabled = this.command.CanExecute(getParameter != null ? getParameter() : null);
            this.button.Text = this.command.Name;

            this.button.Click += OnButtonClick;
            this.command.CanExecuteChanged += OnCommandCanExecuteChanged;
            this.command.PropertyChanged += OnCommandPropertyChanged;
        }

        private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Properties.NAME:
                    this.button.Text = this.command.Name;
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
