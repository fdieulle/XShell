using System;
using System.ComponentModel;
using System.Windows.Forms;
using XShell.Core;

namespace XShell.Demo.Winform.Services.Shell
{
    public static class ShellExtensions
    {
        public static IDisposable Hook(this Button button, IRelayCommand command, Func<object> getParameter = null)
        {
            if (button == null || command == null) return AnonymousDisposable.Empty;

            return new ButtonHooker(button, command, getParameter);
        }

        private class ButtonHooker : IDisposable
        {
            private readonly Button button;
            private readonly IRelayCommand command;
            private readonly Func<object> getParameter;

            public ButtonHooker(Button button, IRelayCommand command, Func<object> getParameter)
            {
                this.button = button;
                this.command = command;
                this.getParameter = getParameter;

                this.button.Click += OnClick;
                this.command.CanExecuteChanged += OnCanExecuteChanged;
                this.command.PropertyChanged += OnPropertyChanged;

                button.Text = command.Name;
            }

            private void OnClick(object sender, EventArgs e)
            {
                object parameter = null;
                if (getParameter != null)
                    parameter = getParameter();
                if(command.CanExecute(parameter))
                    command.Execute(parameter);
            }

            private void OnCanExecuteChanged(object sender, EventArgs e)
            {
                object parameter = null;
                if (getParameter != null)
                    parameter = getParameter();
                this.button.Enabled = command.CanExecute(parameter);
            }

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case "Name":
                        button.Text = command.Name;
                        break;
                    case "IsRunning":
                        button.Enabled = !command.IsRunning;
                        break;
                }
            }

            public void Dispose()
            {
                this.button.Click -= OnClick;
                this.command.CanExecuteChanged -= OnCanExecuteChanged;
                this.command.PropertyChanged -= OnPropertyChanged;
            }
        }

        private class AnonymousDisposable : IDisposable
        {
            private readonly Action action;
            private static readonly IDisposable empty = new AnonymousDisposable(null);
            public static IDisposable Empty{get { return empty; }}

            public AnonymousDisposable(Action action)
            {
                this.action = action;
            }

            #region Implementation of IDisposable

            public void Dispose()
            {
                if (action != null)
                    action();
            }

            #endregion
        }
    }
}
