using System;

namespace XShell
{
    public interface IMenuItem
    {
        string DisplayName { get; set; }

        string IconFilePath { get; set; }

        Action Action { get; set; }

        bool IsEnabled { get; set; }

        bool IsVisible { get; set; }
    }
}