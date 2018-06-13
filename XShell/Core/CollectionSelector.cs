using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XShell.Core
{
    public interface ICollectionSelector : INotifyPropertyChanged
    {
        IEnumerable Items { get; }

        object SelectedItem { get; set; }

        int SelectedIndex { get; set; }
    }

    public interface ICollectionSelector<T> : INotifyPropertyChanged
    {
        ObservableCollection<T> Items { get; }

        T SelectedItem { get; set; }

        int SelectedIndex { get; set; }
    }

    public class CollectionSelector<T> : AbstractNpc, ICollectionSelector<T>, ICollectionSelector
    {
        private bool _skipIndexOf;

        public event DataChanged<T> SelectedItemChanged;

        protected ObservableCollection<T> _items;
        public ObservableCollection<T> Items
        {
            get => _items;
            set
            {
                if (ReferenceEquals(_items, value)) return;
                var oldValue = _items;
                _items = value;

                RaisePropertyChanged(Properties.ItemsPropertyChanged);
                OnItemsChanged(oldValue, value);
            }
        }

        protected T _selectedItem;
        public T SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (Equals(_selectedItem, value)) return;
                var oldValue = _selectedItem;
                _selectedItem = value;
                
                RaisePropertyChanged(Properties.SelectedItemPropertyChanged);
                OnSelectedItemChanged(oldValue, value);
            }
        }

        protected int _selectedIndex = -1;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex == value) return;
                var oldValue = _selectedIndex;
                _selectedIndex = value;

                RaisePropertyChanged(Properties.SelectedIndexPropertyChanged);
                OnSelectedIndexChanged(oldValue, value);
            }
        }

        public CollectionSelector(IEnumerable<T> source = null)
        {
            _items = source != null 
                ? new ObservableCollection<T>(source) 
                : new ObservableCollection<T>();
        }


        protected virtual void OnItemsChanged(IList<T> oldValue, IList<T> newValue)
        {
            
        }

        protected virtual void OnSelectedItemChanged(T oldValue, T newValue)
        {
            SelectedItemChanged?.Invoke(oldValue, newValue);

            if (_items == null || _skipIndexOf) return;
            SelectedIndex = _items.IndexOf(newValue);
        }

        protected virtual void OnSelectedIndexChanged(int oldValue, int newValue)
        {
            if (_items == null) return;

            _skipIndexOf = true;
            SelectedItem = newValue < 0 || newValue > _items.Count ? default(T) : _items[newValue];
            _skipIndexOf = false;
        }

        IEnumerable ICollectionSelector.Items => Items;

        object ICollectionSelector.SelectedItem
        {
            get => SelectedItem;
            set => SelectedItem = (T) value;
        }
    }
}