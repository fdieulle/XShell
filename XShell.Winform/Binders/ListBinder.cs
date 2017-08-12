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
                        var count = this.view.Count - e.NewStartingIndex;
                        var tmp = new object[count];
                        for (var i = 0; i < count; i++)
                        {
                            tmp[count - i - 1] = this.view[this.view.Count - 1];
                            this.view.RemoveAt(i);
                        }

                        // Insert new items
                        foreach (var item in e.NewItems)
                            this.view.Add(item);

                        // Replace copied items after new ones
                        foreach (var item in tmp)
                            this.view.Add(item);
                    }                    
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Count == 1)
                        this.view.RemoveAt(e.OldStartingIndex);
                    else
                    {
                        throw new NotImplementedException();
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems.Count == 1)
                        this.view[e.NewStartingIndex] = e.NewItems[0];
                    else
                    {
                        throw new NotImplementedException();
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
    }
}
