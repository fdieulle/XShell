namespace XShell
{
    public interface IPersistenceService
    {
        string Folder { get; set; }

        void Restore(string name, IPersistable persistable);
        
        void Persist(string name, IPersistable persistable);
    }
}
