using System;
using System.IO;
using System.Text;
using XShell.Services;

namespace XShell.Core
{
    public abstract class AbstractLogic : AbstractNpc, IScreen, IInternalScreen, IPersistable, IDisposable
    {
        private Action _close;

        public string InstanceId { get; private set; }

        #region Implementation of IScreen

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;

                _title = value;
                RaisePropertyChanged(Properties.TitlePropertyChanged);
            }
        }

        #endregion

        #region Implementation of IInternalScreen

        private object _parameter;
        object IInternalScreen.Parameter
        {
            get => _parameter;
            set
            {
                if (_parameter == value) return;
                _parameter = value;
                Setup(_parameter);
            }
        }

        void IInternalScreen.Setup(string instanceId) => InstanceId = instanceId;

        internal virtual void Setup(object param) { }

        void IInternalScreen.Setup(Action onClose) => _close = onClose;

        void IInternalScreen.SerializeParameter(Stream stream) 
            => InternalSerializeParameter(stream);

        internal virtual void InternalSerializeParameter(Stream stream) { }

        object IInternalScreen.DeserializeParameter(Stream stream) 
            => InternalDeserializeParameter(stream);

        internal virtual object InternalDeserializeParameter(Stream stream) 
            => default;

        #endregion

        #region Implementation of IPersistable

        public virtual void Restore(Stream stream) { }

        public virtual void Persist(Stream stream) { }

        #endregion

        protected void Close()
        {
            _close?.Invoke();
        }

        #region Implementation of IDisposable

        public virtual void Dispose() { }

        #endregion
    }
}
