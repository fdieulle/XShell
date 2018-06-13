using System;
using System.ComponentModel;
using System.Windows.Forms;
using XShell.Core;

namespace XShell.Winform.Binders
{
    public class ButtonBinder : IDisposable
    {
        private readonly Button _button;
        private readonly IRelayCommand _command;
        private readonly Func<object> _getParameter;
        private readonly bool _bindName;
        private readonly ToolTip _toolTip;
        private readonly string _toolTipText;

        public ButtonBinder(Button button, IRelayCommand command, 
            Func<object> getParameter = null, bool bindName = true, 
            ToolTip toolTip = null, string toolTipText = null)
        {
            _button = button;
            _command = command;
            _getParameter = getParameter;
            _bindName = bindName;
            _toolTip = toolTip;
            _toolTipText = toolTipText;

            _button.Enabled = _command.CanExecute(getParameter?.Invoke());
            if (_bindName)
                _button.Text = _command.Name;
            toolTip?.SetToolTip(_button, toolTipText ?? _command.Name);

            _button.Click += OnButtonClick;
            _command.CanExecuteChanged += OnCommandCanExecuteChanged;
            _command.PropertyChanged += OnCommandPropertyChanged;
            _button.Disposed += OnButtonDisposed;
        }

        private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Core.Properties.Name:
                    if (_bindName)
                        _button.Text = _command.Name;
                    if(_toolTipText == null)
                        _toolTip?.SetToolTip(_button, _command.Name);
                    break;
            }
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            _button.Enabled = _command.CanExecute(_getParameter?.Invoke());
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            _command.Execute(_getParameter?.Invoke());
        }

        private void OnButtonDisposed(object sender, EventArgs e)
        {
            Dispose();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _button.Disposed -= OnButtonDisposed;
            _button.Click -= OnButtonClick;
            _command.CanExecuteChanged -= OnCommandCanExecuteChanged;
            _command.PropertyChanged -= OnCommandPropertyChanged;
        }

        #endregion
    }
}
