using System.IO;

namespace XShell.Tools
{
    public sealed class MemoryStreamNoDispose : MemoryStream
    {
        protected override void Dispose(bool disposing) { }
    }
}
