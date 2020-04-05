using System.IO;
using System.Text;

namespace XShell.Core
{
    public abstract class AbstractParameterizedLogic<TParameter> : AbstractLogic
        where TParameter : class
    {
        internal sealed override void Setup(object param)
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

        protected virtual void OnParameterChanged(TParameter oldParameter, TParameter newParameter) { }

        internal sealed override void InternalSerializeParameter(Stream stream)
            => SerializeParameter(stream);

        protected virtual void SerializeParameter(Stream stream) { }

        internal sealed override object InternalDeserializeParameter(Stream stream)
            => DeserializeParameter(stream);

        protected virtual TParameter DeserializeParameter(Stream stream) => default;
    }

    public abstract class AbstractParameterizedLogic : AbstractParameterizedLogic<object>
    {

    }
}
