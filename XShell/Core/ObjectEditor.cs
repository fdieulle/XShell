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
        private readonly Func<T, T> _clone;

        public event DataChanged<T> ApplyExecuted;
        public event DataChanged<T> CancelExecuted;

        private T _origin;
        public T Object
        {
            get => _origin;
            set
            {
                if (ReferenceEquals(_origin, value)) return;
                _origin = value;
                RaisePropertyChanged(Properties.ObjectPropertyChanged);
                Editable = _clone(value);
            }
        }

        private T _editable;
        public T Editable
        {
            get => _editable;
            set
            {
                if (ReferenceEquals(_editable, value)) return;
                _editable = value;
                RaisePropertyChanged(Properties.EditablePropertyChanged);
            }
        }

        public IRelayCommand ApplyCommand { get; }

        public IRelayCommand CancelCommand { get; }

        public ObjectEditor(
            Func<T, T> clone = null,
            T data = default)
        {
            _clone = clone ?? (p => p);
            _origin = data;

            ApplyCommand = new RelayCommand(ExecuteApplyCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);
        }

        private void ExecuteApplyCommand(object parameter)
        {
            var oldValue = _origin;
            Object = _editable;
            Raise(ApplyExecuted, oldValue, _origin);
        }

        private void ExecuteCancelCommand(object parameter)
        {
            var oldValue = _editable;
            Editable = _clone(_origin);
            Raise(CancelExecuted, oldValue, _editable);
        }

        private void Raise(DataChanged<T> handler, T oldValue, T newValue)
        {
            if (handler == null) return;
            handler(oldValue, newValue);
        }

        object IObjectEditor.Object
        {
            get => Object;
            set => Object = (T)value;
        }

        object IObjectEditor.Editable
        {
            get => Editable;
            set => Editable = (T)value;
        }
    }
}
