using System;

namespace XShell.Services
{
    internal interface IInternalScreen
    {
        object Parameter { get; set; }

        void Setup(string instanceId);

        void Setup(Action onClose);
    }
}
