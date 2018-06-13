using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace XShell.Winform.Binders
{
    public abstract class AbstractOneWayBinder<TControl, TProperty> : IDisposable
        where TControl : Control
    {
        private readonly string _propertyName;
        private readonly Func<object, TProperty> _getPropertyValue;
        private bool _isDisposed;

        protected TControl Control { get; }

        protected object DataContext { get; }

        protected AbstractOneWayBinder(TControl control, object dataContext, string propertyName)
        {
            Control = control;
            DataContext = dataContext;
            _propertyName = propertyName;
            _getPropertyValue = dataContext.BuildPropertyGetter<TProperty>(propertyName);

            if (DataContext is INotifyPropertyChanged npc)
                npc.PropertyChanged += OnDataContextPropertyChanged;
            Control.Disposed += OnControlDisposed;

            // ReSharper disable once VirtualMemberCallInConstructor
            UpdateControl(_getPropertyValue(dataContext));
        }

        private void OnDataContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == null || e.PropertyName == _propertyName)
                UpdateControl(_getPropertyValue(DataContext));
        }

        protected abstract void UpdateControl(TProperty value);

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

            if (DataContext is INotifyPropertyChanged npc)
                npc.PropertyChanged -= OnDataContextPropertyChanged;

            Disposing();
        }

        protected virtual void Disposing() { }

        #endregion
    }
}
