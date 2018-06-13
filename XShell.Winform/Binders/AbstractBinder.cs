using System;
using System.Windows.Forms;

namespace XShell.Winform.Binders
{
    public abstract class AbstractBinder<TControl> : IDisposable
        where TControl : Control
    {
        private bool _isDisposed;

        protected TControl Control { get; }

        protected AbstractBinder(TControl control)
        {
            Control = control;
            Control.Disposed += OnControlDisposed;
        }

        private void OnControlDisposed(object sender, EventArgs e)
        {
            Dispose();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            Control.Disposed -= OnControlDisposed;

            Disposing();
        }

        protected virtual void Disposing() { }

        #endregion
    }
}