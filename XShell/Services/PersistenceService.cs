using System;
using System.IO;
using System.Text;

namespace XShell.Services
{
    public class PersistenceService : IPersistenceService
    {
        #region Implementation of IPersistenceService

        public string Folder { get; set; }

        public void Restore(string name, IPersistable persistable)
        {
            if (persistable == null) return;

            try
            {
                var filePath = GetFilePath(Folder, name);
                if (!File.Exists(filePath)) return;

                using (var reader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    persistable.Restore(reader);
                }
            }
            catch (Exception e)
            {
                WriteException(name, "Exception happens during restore", e);
            }
        }

        public void Persist(string name, IPersistable persistable)
        {
            if (persistable == null) return;

            try
            {
                var folder = GetFolder();

                // Process the persistence in 2 times to avoid to corrupt persisted files.
                using (var ms = new MemoryStream())
                {
                    persistable.Persist(ms);

                    var buffer = ms.GetBuffer();
                    var length = (int) ms.Length;
                    if (length == 0) return;

                    var filePath = GetFilePath(folder, name);
                    using (var writer = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                        writer.Write(buffer, 0, length);
                }

                
            }
            catch (Exception e)
            {
                WriteException(name, "Exception happens during persist", e);
            }
        }

        private void WriteException(string name, string message, Exception e)
        {
            try
            {
                var folder = GetFolder();

                var sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                sb.Append(" - ");
                sb.Append(message ?? string.Empty);
                sb.AppendException(e);

                var filePath = GetFilePath(folder, name, "ex");
                File.AppendAllText(filePath, sb.ToString());

// ReSharper disable EmptyGeneralCatchClause
            } catch
// ReSharper restore EmptyGeneralCatchClause
            {}
        }

        private string GetFolder()
        {
            var folder = Folder ?? @".\";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        private static string GetFilePath(string folder, string name, string extension = "data") 
            => Path.Combine(folder ?? @".\", $"{name ?? string.Empty}.{extension ?? "data"}");

        #endregion
    }
}