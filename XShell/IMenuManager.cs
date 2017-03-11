using System;

namespace XShell
{
    public interface IMenuManager
    {
        void Add(string path, Action action = null, string displayName = null, string iconFilePath = null, bool isEnabled = true, bool isVisible = true);

        IMenuItem Get(string path);

        void Remove(string path);
    }
}
