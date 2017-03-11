using System.IO;

namespace XShell.Mvvm
{
    public abstract class AbstractPersistableScreenViewModel : AbstractScreenViewModel, IPersistable
    {
        #region Implementation of IPersistable

        public abstract void Restore(Stream stream);

        public abstract void Persist(Stream stream);

        #endregion
    }
}
