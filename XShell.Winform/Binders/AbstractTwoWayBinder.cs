using System;
using System.Windows.Forms;

namespace XShell.Winform.Binders
{
    public abstract class AbstractTwoWayBinder<TControl, TProperty> : AbstractOneWayBinder<TControl, TProperty>
        where TControl : Control
    {
        private readonly Action<object, TProperty> _setPropertyValue;

        protected AbstractTwoWayBinder(TControl control, object dataContext, string propertyName)
            : base(control, dataContext, propertyName)
        {
            _setPropertyValue = dataContext.BuildPropertySetter<TProperty>(propertyName);
        }

        protected void UpdateDataContext(TProperty value)
        {
            _setPropertyValue(DataContext, value);
        }
    }
}