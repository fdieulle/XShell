using System.ComponentModel;

namespace XShell.Core
{
    public abstract class AbstractParameterizedLogic<TParameter> : AbstractLogic
        where TParameter : class
    {
        internal override void Setup(object param)
        {
            base.Setup(parameter);
            Parameter = param as TParameter;
        }

        private static readonly PropertyChangedEventArgs parameterPropertyChanged = new PropertyChangedEventArgs("Parameter");
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
                RaisePropertyChanged(parameterPropertyChanged);
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
