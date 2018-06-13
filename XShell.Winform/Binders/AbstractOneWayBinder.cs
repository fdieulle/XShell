using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace XShell.Winform.Binders
{
    public abstract class AbstractOneWayBinder<TControl, TProperty> : AbstractBinder<TControl>
        where TControl : Control
    {
        private readonly string _propertyName;
        private readonly Func<object, TProperty> _getPropertyValue;

        protected object DataContext { get; }

        protected AbstractOneWayBinder(TControl control, object dataContext, string propertyName)
            : base(control)
        {
            DataContext = dataContext;
            _propertyName = propertyName;
            _getPropertyValue = dataContext.BuildPropertyGetter<TProperty>(propertyName);

            if (DataContext is INotifyPropertyChanged npc)
                npc.PropertyChanged += OnDataContextPropertyChanged;

            // ReSharper disable once VirtualMemberCallInConstructor
            UpdateControl(_getPropertyValue(dataContext));
        }

        private void OnDataContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == null || e.PropertyName == _propertyName)
                UpdateControl(_getPropertyValue(DataContext));
        }

        protected abstract void UpdateControl(TProperty value);

        protected override void Disposing()
        {
            if (DataContext is INotifyPropertyChanged npc)
                npc.PropertyChanged -= OnDataContextPropertyChanged;
        }
    }
}
