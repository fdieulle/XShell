using System.IO;

namespace XShell
{
    public interface IPersistable
    {
        void Restore(Stream stream);

        void Persist(Stream stream);
    }
}