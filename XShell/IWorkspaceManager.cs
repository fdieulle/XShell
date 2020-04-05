using System.Collections.Generic;

namespace XShell
{
    public interface IWorkspaceManager
    {
        string Folder { get; set; }

        IEnumerable<string> GetAllWorkspace();

        void Save(string name = null);

        void Load(string name = null);
    }
}
