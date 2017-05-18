using System;
using System.ComponentModel;
using System.IO;
using XShell.Services;

namespace XShell.Core
{
    public abstract class AbstractLogic : AbstractNpc, IScreen, IInternalScreen, IPersistable, IDisposable
    {
        private Action close;

        public string InstanceId { get; private set; }

        #region Implementation of IScreen

        private static readonly PropertyChangedEventArgs titlePropertyChanged = new PropertyChangedEventArgs("Title");
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title == value) return;

                title = value;
                RaisePropertyChanged(titlePropertyChanged);
            }
        }

        #endregion

        #region Implementation of IInternalScreen

        private object parameter;
        object IInternalScreen.Parameter
        {
            get { return parameter; }
            set
            {
                if (parameter == value) return;
                parameter = value;
                Setup(parameter);
            }
        }

        void IInternalScreen.Setup(string instanceId)
        {
            InstanceId = instanceId;
        }

        internal virtual void Setup(object param) { }

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
