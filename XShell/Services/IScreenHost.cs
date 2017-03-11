using System;

namespace XShell.Services
{
    public interface IScreenHost
    {
        event Action<IScreenHost> ScreenClosed;

        string Title { set; }

        void Close();

        void BringToFront();
    }
}