using System.IO;

namespace XShell.Mvp
{
    public abstract class AbstractPersistableScreenPresenter : AbstractScreenPresenter, IPersistable
    {
        #region Implementation of IPersistable

        public abstract void Restore(Stream stream);

        public abstract void Persist(Stream stream);

        #endregion
    }
}
