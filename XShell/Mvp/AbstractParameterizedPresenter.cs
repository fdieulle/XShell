namespace XShell.Mvp
{
    public abstract class AbstractParameterizedPresenter<TParameter> : AbstractPresenter
        where TParameter : class
    {
        internal override void Setup(object param)
        {
            base.Setup(parameter);
            Parameter = param as TParameter;
        }

        private TParameter parameter;

        public TParameter Parameter
        {
            get { return parameter; }
            set
            {
                if (parameter == value) return;
                var oldParameter = parameter;
                parameter = value;
                OnParameterChanged(oldParameter, value);
            }
        }

        protected virtual void OnParameterChanged(TParameter oldParameter, TParameter newParameter)
        {

        }
    }

    public abstract class AbstractParameterizedPresenter : AbstractParameterizedPresenter<object>
    {
        
    }
}
