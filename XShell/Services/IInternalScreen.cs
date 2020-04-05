using System;
using System.IO;

namespace XShell.Services
{
    internal interface IInternalScreen
    {
        object Parameter { get; set; }

        void Setup(string instanceId);

        void Setup(Action onClose);

        void SerializeParameter(Stream stream);

        object DeserializeParameter(Stream stream);
    }
}
