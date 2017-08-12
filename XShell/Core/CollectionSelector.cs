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
        IList<T> Items { get; }

        T SelectedItem { get; set; }

        int SelectedIndex { get; set; }
    }

    public class CollectionSelector<T> : AbstractNpc, ICollectionSelector<T>, ICollectionSelector
    {
        private bool isIndexChanging;

        public event DataChanged<T> SelectedItemChanged; 

        protected IList<T> items;
        public IList<T> Items
        {
            get { return items; }
            set
            {
                if (ReferenceEquals(this.items, value)) return;
                var oldValue = this.items;
                this.items = value;

                this.RaisePropertyChanged(Properties.ItemsPropertyChanged);
                this.OnItemsChanged(oldValue, value);
            }
        }

        protected T selectedItem;
        public T SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (Equals(this.selectedItem, value)) return;
                var oldValue = this.selectedItem;
                this.selectedItem = value;
                
                this.RaisePropertyChanged(Properties.SelectedItemPropertyChanged);
                this.OnSelectedItemChanged(oldValue, value);
            }
        }

        protected int selectedIndex = -1;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (this.selectedIndex == value) return;
                var oldValue = this.selectedIndex;
                this.selectedIndex = value;

                this.RaisePropertyChanged(Properties.SelectedIndexPropertyChanged);
                OnSelectedIndexChanged(oldValue, value);
            }
        }

        public CollectionSelector(IEnumerable<T> source = null)
            : this(source != null ? new ObservableCollection<T>(source) : null) { }

        public CollectionSelector(IList<T> source)
        {
            this.items = source;
            if (this.items != null && this.items.Count > 0)
                SelectedIndex = 0;
        }

        protected virtual void OnItemsChanged(IList<T> oldValue, IList<T> newValue)
        {
            
        }

        protected virtual void OnSelectedItemChanged(T oldValue, T newValue)
        {
            var handler = this.SelectedItemChanged;
            if (handler != null) this.SelectedItemChanged(oldValue, newValue);

            if (this.items == null || this.isIndexChanging) return;
            this.SelectedIndex = this.items.IndexOf(newValue);
        }

        protected virtual void OnSelectedIndexChanged(int oldValue, int newValue)
        {
            if (this.items == null) return;

            this.isIndexChanging = true;
            this.SelectedItem = newValue < 0 || newValue > this.items.Count ? default(T) : this.items[newValue];
            this.isIndexChanging = false;
        }

        IEnumerable ICollectionSelector.Items
        {
            get { return Items; }
        }

        object ICollectionSelector.SelectedItem
        {
            get { return SelectedItem; }
            set { SelectedItem = (T) value; }
        }
    }
}