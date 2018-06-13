using System;
using System.Collections.Generic;

namespace XShell.Services
{
    public abstract class AbstractMenuManager<TMenuItem> : IMenuManager
        where TMenuItem : class, IMenuItem
    {
        private readonly Node _root = new Node(null, null);
        
        #region Implementation of IMenuManager

        public void Add(string path, Action action = null, string displayName = null, string iconFilePath = null, bool isEnabled = true, bool isVisible = true)
        {
            if (string.IsNullOrEmpty(path)) return;

            var node = _root;
            var split = path.Split('/');
            foreach (var item in split)
            {
                if(string.IsNullOrEmpty(item)) continue;
                Node child;
                if (!node._children.TryGetValue(item, out child))
                {
                    var menuItem = CreateMenuItem(node._item);
                    menuItem.DisplayName = item;
                    node._children.Add(item, child = new Node(node, menuItem));
                }
                
                node = child;
            }

            node._item.Action = action;
            node._item.DisplayName = displayName ?? split[split.Length - 1];
            node._item.IconFilePath = iconFilePath;
            node._item.IsEnabled = isEnabled;
            node._item.IsVisible = isVisible;
        }

        public IMenuItem Get(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var node = _root;
            var split = path.Split('/');
            foreach (var item in split)
            {
                if (string.IsNullOrEmpty(item)) continue;
                Node child;
                if (!node._children.TryGetValue(item, out child))
                    return null;

                node = child;
            }

            return node._item;
        }

        public void Remove(string path)
        {
            var node = _root;
            var split = path.Split('/');
            foreach (var item in split)
            {
                if (string.IsNullOrEmpty(item)) continue;
                Node child;
                if (!node._children.TryGetValue(item, out child))
                    return;

                node = child;
            }

            DeleteMenuItem(node._parent != null ? node._parent._item : null, node._item);
        }

        #endregion

        protected abstract TMenuItem CreateMenuItem(TMenuItem parent);

        protected abstract void DeleteMenuItem(TMenuItem parent, TMenuItem item);

        protected class Node
        {
            public readonly Node _parent;
            public readonly Dictionary<string, Node> _children = new Dictionary<string, Node>();

            public readonly TMenuItem _item;

            public Node(Node parent, TMenuItem item)
            {
                _parent = parent;
                _item = item;
            }
        }
    }
}