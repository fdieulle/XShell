using System.IO;

namespace XShell.Mvc
{
    public abstract class AbstractPersistableScreenController : AbstractScreenController, IPersistable
    {
        #region Implementation of IPersistable

        public abstract void Restore(Stream stream);

        public abstract void Persist(Stream stream);

        #endregion
    }
}
