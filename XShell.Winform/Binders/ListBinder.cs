using System;
using System.Collections;
using System.Collections.Specialized;

namespace XShell.Winform.Binders
{
    public class ListBinder : IDisposable
    {
        private readonly IList _view;
        private readonly IEnumerable _logic;

        public ListBinder(IList view, IEnumerable logic)
        {
            _view = view;
            _logic = logic;

            _view.Clear();
            if (logic != null)
            {
                foreach (var item in _logic)
                    _view.Add(item);
            }

            if (logic is INotifyCollectionChanged ncc)
                ncc.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    _view.Clear();
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex >= _view.Count)
                    {
                        foreach (var item in e.NewItems)
                            _view.Add(item);
                    }
                    else if (e.NewItems.Count == 1)
                        _view.Insert(e.NewStartingIndex, e.NewItems[0]);
                    else
                    {
                        // Copy all items which has to be moved and remove them from the list
                        var lasts = CopyAndRemoveLastItems(_view.Count - e.NewStartingIndex);

                        // Insert new items
                        for (var i = 0; i < e.NewItems.Count; i++)
                            _view.Add(e.NewItems[i]);

                        // Replace copied items after new ones
                        for (var i = 0; i < lasts.Length; i++)
                            _view.Add(lasts[i]);
                    }                    
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems.Count == 1)
                        _view.RemoveAt(e.OldStartingIndex);
                    else
                    {
                        // Copy all items which has to be moved and remove them from the list
                        var lasts = CopyAndRemoveLastItems(_view.Count - (e.OldStartingIndex + e.OldItems.Count));

                        // Remove old items
                        for(var i = e.OldStartingIndex + e.OldItems.Count; i >= e.OldStartingIndex; i--)
                            _view.RemoveAt(i);

                        // Replace copied items after new ones
                        for (var i = 0; i < lasts.Length;  i++)
                            _view.Add(lasts[i]);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems.Count == 1 && e.NewStartingIndex >= 0)
                        _view[e.NewStartingIndex] = e.NewItems[0];
                    else if (e.NewStartingIndex >= 0)
                    {
                        for (var i = 0; i < e.NewItems.Count; i++)
                            _view[e.NewStartingIndex + i] = e.NewItems[i];
                    }
                    else
                    {
                        for (var i = 0; i < e.NewItems.Count; i++)
                        {
                            var index = _view.IndexOf(e.NewItems[i]);
                            _view[index] = e.NewItems[i];
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.NewItems.Count == 1)
                    {
                        var swap = _view[e.OldStartingIndex];
                        _view[e.OldStartingIndex] = _view[e.NewStartingIndex];
                        _view[e.NewStartingIndex] = swap;
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
            if (_logic is INotifyCollectionChanged ncc)
                ncc.CollectionChanged -= OnCollectionChanged;
        }

        #endregion

        private object[] CopyAndRemoveLastItems(int count)
        {
            var array = new object[count];
            for (var i = 0; i < count; i++)
            {
                array[count - i - 1] = _view[_view.Count - 1];
                _view.RemoveAt(i);
            }
            return array;
        }
    }
}
