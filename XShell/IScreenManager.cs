using System;

namespace XShell
{
    public interface IScreenManager : IDisposable
    {
        void Display(Type idType, string instanceId = null, object parameter = null);

        void Popup(Type idType, string instanceId = null, object parameter = null, PopupAttribute popupAttribute = null);

        void SetParameter(Type idType, string instanceId, object parameter);

        object GetParameter(Type idType, string instanceId);

        void Close(Type idType, string instanceId = null);

        void CloseAll(Type idType = null);
    }
}