using System;

namespace XShell
{
    public interface IScreenManager : IDisposable
    {
        void Display(Type idType, string instanceId = null, object parameter = null);

        void Popup(Type idType, string instanceId = null, object parameter = null);

        void Close(Type idType, string instanceId = null);

        void CloseAll(Type idType);
    }
}