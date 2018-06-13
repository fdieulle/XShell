using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace XShell.Winform.Binders
{
    public class ProgressBarBinder : AbstractTwoWayBinder<ProgressBar, int>
    {
        private readonly string _minPropertyName;
        private readonly Func<object, int> _getMinValue;

        private readonly string _maxPropertyName;
        private readonly Func<object, int> _getMaxValue;

        public ProgressBarBinder(ProgressBar control, object dataContext, string propertyName, string minPropertyName, string maxPropertyName) 
            : base(control, dataContext, propertyName)
        {
            _minPropertyName = minPropertyName;
            _maxPropertyName = maxPropertyName;

            _getMinValue = dataContext.BuildPropertyGetter<int>(minPropertyName);
            _getMaxValue = dataContext.BuildPropertyGetter<int>(maxPropertyName);

            if (_getMinValue != null)
                Control.Minimum = _getMinValue(DataContext);
            if (_getMaxValue != null)
                Control.Minimum = _getMaxValue(DataContext);
        }

        #region Overrides of AbstractOneWayBinder<ProgressBar,double>

        #region Overrides of AbstractOneWayBinder<ProgressBar,int>

        protected override void OnDataContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnDataContextPropertyChanged(sender, e);

            if (e.PropertyName == null)
            {
                if(_getMinValue != null)
                    Control.Minimum = _getMinValue(DataContext);
                if (_getMaxValue != null)
                    Control.Minimum = _getMaxValue(DataContext);
            }
            else if(e.PropertyName == _minPropertyName)
            {
                if (_getMinValue != null)
                    Control.Minimum = _getMinValue(DataContext);
            }
            else if (e.PropertyName == _maxPropertyName)
            {
                if (_getMaxValue != null)
                    Control.Maximum = _getMaxValue(DataContext);
            }
        }

        #endregion

        protected override void UpdateControl(int value)
        {
            Control.Value = value;
        }

        #endregion
    }
}
