using System;
using System.IO;

namespace XShell.Services
{
    public abstract class AbstractLogic : IInternalScreen, IPersistable, IDisposable
    {
        private Action close;

        public string InstanceId { get; private set; }

        #region Implementation of IInternalScreen

        void IInternalScreen.Setup(string instanceId, object parameter)
        {
            InstanceId = instanceId;
            Setup(parameter);
        }

        internal virtual void Setup(object parameter) { }

        void IInternalScreen.Setup(Action onClose)
        {
            close = onClose;
        }

        #endregion

        #region Implementation of IPersistable

        public virtual void Restore(Stream stream) { }

        public virtual void Persist(Stream stream) { }

        #endregion

        protected void Close()
        {
            if (close != null)
                close();
        }

        #region Implementation of IDisposable

        public virtual void Dispose() { }

        #endregion
    }
}
