using System;

namespace XShell
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ScreenMenuItemAttribute : Attribute
    {
        public string Path { get; private set; }

        public string DisplayName { get; set; }

        public string IconFilePath { get; set; }

        public bool IsPopup { get; set; }

        public ScreenMenuItemAttribute(string path)
        {
            Path = path;
        }
    }
}