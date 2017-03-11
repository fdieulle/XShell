using System;
using System.Collections.Generic;

namespace XShell.Services
{
    public abstract class AbstractMenuManager<TMenuItem> : IMenuManager
        where TMenuItem : class, IMenuItem
    {
        private readonly Node root = new Node(null, null);
        
        #region Implementation of IMenuManager

        public void Add(string path, Action action = null, string displayName = null, string iconFilePath = null, bool isEnabled = true, bool isVisible = true)
        {
            if (string.IsNullOrEmpty(path)) return;

            var node = root;
            var split = path.Split('/');
            foreach (var item in split)
            {
                if(string.IsNullOrEmpty(item)) continue;
                Node child;
                if (!node.children.TryGetValue(item, out child))
                {
                    var menuItem = CreateMenuItem(node.item);
                    menuItem.DisplayName = item;
                    node.children.Add(item, child = new Node(node, menuItem));
                }
                
                node = child;
            }

            node.item.Action = action;
            node.item.DisplayName = displayName ?? split[split.Length - 1];
            node.item.IconFilePath = iconFilePath;
            node.item.IsEnabled = isEnabled;
            node.item.IsVisible = isVisible;
        }

        public IMenuItem Get(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var node = root;
            var split = path.Split('/');
            foreach (var item in split)
            {
                if (string.IsNullOrEmpty(item)) continue;
                Node child;
                if (!node.children.TryGetValue(item, out child))
                    return null;

                node = child;
            }

            return node.item;
        }

        public void Remove(string path)
        {
            var node = root;
            var split = path.Split('/');
            foreach (var item in split)
            {
                if (string.IsNullOrEmpty(item)) continue;
                Node child;
                if (!node.children.TryGetValue(item, out child))
                    return;

                node = child;
            }

            DeleteMenuItem(node.parent != null ? node.parent.item : null, node.item);
        }

        #endregion

        protected abstract TMenuItem CreateMenuItem(TMenuItem parent);

        protected abstract void DeleteMenuItem(TMenuItem parent, TMenuItem item);

        protected class Node
        {
            public readonly Node parent;
            public readonly Dictionary<string, Node> children = new Dictionary<string, Node>();

            public readonly TMenuItem item;

            public Node(Node parent, TMenuItem item)
            {
                this.parent = parent;
                this.item = item;
            }
        }
    }
}