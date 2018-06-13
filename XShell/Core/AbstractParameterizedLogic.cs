namespace XShell.Core
{
    public abstract class AbstractParameterizedLogic<TParameter> : AbstractLogic
        where TParameter : class
    {
        internal override void Setup(object param)
        {
            base.Setup(_parameter);
            Parameter = param as TParameter;
        }

        private TParameter _parameter;
        public TParameter Parameter
        {
            get => _parameter;
            set
            {
                if (_parameter == value) return;
                var oldParameter = _parameter;
                _parameter = value;
                OnParameterChanged(oldParameter, value);
                RaisePropertyChanged(Properties.ParameterPropertyChanged);
            }
        }

        protected virtual void OnParameterChanged(TParameter oldParameter, TParameter newParameter)
        {

        }
    }

    public abstract class AbstractParameterizedLogic : AbstractParameterizedLogic<object>
    {

    }
}
