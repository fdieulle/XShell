using System;

namespace XShell
{
    public interface IScreen
    {
        event Action TitleChanged;

        string Title { get; }
    }
}
