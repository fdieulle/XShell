using System;

namespace XShell
{
    public enum StartupLocation
    {
        CenterScreen,
        CenterOwner,
        Manual,
        MousePosition
    }

    public enum ResizeMode
    {
        NoResize,
        CanResizeWithGrip,
        CanResize,
        AutoSize
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class PopupAttribute : Attribute
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public StartupLocation StartupLocation { get; set; }

        public double Top { get; set; }

        public double Left { get; set; }

        public string Icon { get; set; }

        public bool TopMost { get; set; }

        public ResizeMode ResizeMode { get; set; }
    }
}
