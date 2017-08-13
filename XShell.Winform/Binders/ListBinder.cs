using System;
using System.Collections;
using System.Collections.Specialized;

namespace XShell.Winform.Binders
{
    public class ListBinder : IDisposable
    {
        private readonly IList view;
        private readonly IEnumerable logic;

        public ListBinder(IList view, IEnumerable logic)
        {
            this.view = view;
            this.logic = logic;

            this.view.Clear();
            foreach (var item in this.logic)
                this.view.Add(item);

            var ncc = logic as INotifyCollectionChanged;
            if (ncc != null)
                ncc.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    this.view.Clear();
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex >= this.view.Count)
                    {
                        foreach (var item in e.NewItems)
                            this.view.Add(item);
                    }
                    else if (e.NewItems.Count == 1)
                        this.view.Insert(e.NewStartingIndex, e.NewItems[0]);
                    else
                    {
                        // Copy all items which has to be moved and remove them from the list
                        var lasts = CopyAndRemoveLastItems(this.view.Count - e.NewStartingIndex);

                        // Insert new items
                        for (var i = 0; i < e.NewItems.Count; i++)
                            this.view.Add(e.NewItems[i]);

                        // Replace copied items after new ones
                        for (var i = 0; i < lasts.Length; i++)
                            this.view.Add(lasts[i]);
                    }                    
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Count == 1)
                        this.view.RemoveAt(e.OldStartingIndex);
                    else
                    {
                        // Copy all items which has to be moved and remove them from the list
                        var lasts = CopyAndRemoveLastItems(this.view.Count - (e.OldStartingIndex + e.OldItems.Count));

                        // Remove old items
                        for(var i = e.OldStartingIndex + e.OldItems.Count; i >= e.OldStartingIndex; i--)
                            this.view.RemoveAt(i);

                        // Replace copied items after new ones
                        for (var i = 0; i < lasts.Length;  i++)
                            this.view.Add(lasts[i]);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems.Count == 1 && e.NewStartingIndex >= 0)
                        this.view[e.NewStartingIndex] = e.NewItems[0];
                    else if (e.NewStartingIndex >= 0)
                    {
                        for (var i = 0; i < e.NewItems.Count; i++)
                            this.view[e.NewStartingIndex + i] = e.NewItems[i];
                    }
                    else
                    {
                        for (var i = 0; i < e.NewItems.Count; i++)
                        {
                            var index = this.view.IndexOf(e.NewItems[i]);
                            this.view[index] = e.NewItems[i];
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.NewItems.Count == 1)
                    {
                        var swap = this.view[e.OldStartingIndex];
                        this.view[e.OldStartingIndex] = this.view[e.NewStartingIndex];
                        this.view[e.NewStartingIndex] = swap;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    break;
            }
        }
        
        #region Implementation of IDisposable

        public void Dispose()
        {
            var ncc = this.logic as INotifyCollectionChanged;
            if (ncc != null)
                ncc.CollectionChanged -= OnCollectionChanged;
        }

        #endregion

        private object[] CopyAndRemoveLastItems(int count)
        {
            var array = new object[count];
            for (var i = 0; i < count; i++)
            {
                array[count - i - 1] = this.view[this.view.Count - 1];
                this.view.RemoveAt(i);
            }
            return array;
        }
    }
}
