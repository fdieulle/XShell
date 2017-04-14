using System;

namespace XShell.Services
{
    internal interface IInternalScreen
    {
        void Setup(string instanceId, object parameter);

        void Setup(Action onClose);
    }
}
