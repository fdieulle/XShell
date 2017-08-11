using System;
using System.ComponentModel;

namespace XShell.Core
{
    public delegate void DataChanged<in T>(T oldValue, T newValue);

    public interface IObjectEditor : INotifyPropertyChanged
    {
        object Object { get; set; }

        object Editable { get; set; }

        IRelayCommand ApplyCommand { get; }

        IRelayCommand CancelCommand { get; }
    }

    public class ObjectEditor<T> : AbstractNpc, IObjectEditor
    {
        private readonly Func<T, T> clone;

        public event DataChanged<T> ApplyExecuted;
        public event DataChanged<T> CancelExecuted;

        private T origin;
        public T Object
        {
            get { return origin; }
            set
            {
                if (ReferenceEquals(origin, value)) return;
                origin = value;
                RaisePropertyChanged(Properties.ObjectPropertyChanged);
                Editable = clone(value);
            }
        }

        private T editable;
        public T Editable
        {
            get { return editable; }
            set
            {
                if (ReferenceEquals(editable, value)) return;
                editable = value;
                RaisePropertyChanged(Properties.EditablePropertyChanged);
            }
        }

        public IRelayCommand ApplyCommand { get; private set; }

        public IRelayCommand CancelCommand { get; private set; }

        public ObjectEditor(
            Func<T, T> clone = null,
            T data = default(T))
        {
            this.clone = clone ?? (p => p);
            origin = data;

            ApplyCommand = new RelayCommand(ExecuteApplyCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
        }

        private void ExecuteApplyCommand(object parameter)
        {
            var oldValue = origin;
            Object = editable;
            Raise(ApplyExecuted, oldValue, origin);
        }

        private void ExecuteCancelCommand(object parameter)
        {
            var oldValue = editable;
            Editable = clone(origin);
            Raise(CancelExecuted, oldValue, editable);
        }

        private void Raise(DataChanged<T> handler, T oldValue, T newValue)
        {
            if (handler == null) return;
            handler(oldValue, newValue);
        }

        object IObjectEditor.Object
        {
            get { return Object; } 
            set { Object = (T)value; }
        }

        object IObjectEditor.Editable
        {
            get { return Editable; }
            set { Editable = (T)value; }
        }
    }
}
