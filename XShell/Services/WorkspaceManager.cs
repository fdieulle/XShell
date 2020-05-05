using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XShell.Tools;

namespace XShell.Services
{
    public class WorkspaceManager<TBaseView, TScreen, TPopup> : IWorkspaceManager
        where TBaseView : class
        where TScreen : IScreenHost
        where TPopup : IPopupScreenHost
    {
        private readonly IMainWindow<TScreen> _mainWindow;
        private readonly AbstractScreenManager<TBaseView, TScreen, TPopup> _screenManager;
        private readonly IViewBox _viewBox;

        public WorkspaceManager(
            IMainWindow<TScreen> mainWindow, 
            AbstractScreenManager<TBaseView, TScreen, TPopup> screenManager,
            IViewBox viewBox)
        {
            _mainWindow = mainWindow;
            _screenManager = screenManager;
            _viewBox = viewBox;
        }

        public string Folder { get; set; }

        public IEnumerable<string> GetAllWorkspace()
        {
            var folder = GetFolder();
            if (!Directory.Exists(GetFolder()))
                yield return "default";
            else
            {
                foreach (var file in Directory.GetFiles(folder, $"*.workspace").Select(p => new FileInfo(p)))
                    yield return file.Name;
            }
        }

        public void Save(string name = null)
        {
            try
            {
                name = name ?? GetCurrentWorkspace();

                var file = GetFile(name, true);
                using (var writer = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    var binaryWriter = new BinaryWriter(writer);
                    binaryWriter.Write(1); // Write version

                    var memory = new MemoryStreamNoDispose();
                    _screenManager.SaveWorkspace(memory);
                    memory.Commit(writer);

                    binaryWriter.Write(_mainWindow.GetPositionAndSize() ?? new RectangleSettings { Width = 300, Height = 300 });

                    _mainWindow.SaveWorkspace(memory);
                    memory.Commit(writer);
                }

                File.WriteAllText(GetCurrentFile(), name);
            }
            catch (Exception e)
            {
                ShowError($"Workspace {name} can't be saved !", e);
            }
        }

        public void Load(string name = null)
        {
            try
            {
                name = name ?? GetCurrentWorkspace();
                var file = GetFile(name);
                if (!File.Exists(file))
                {
                    // Todo: log warn: ($"Workspace {name} not found !", "Workspace issue", ViewboxButtons.Ok, ViewboxImage.Error);
                    return;
                }

                using (var reader = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    _screenManager.CloseAll();
                    var binaryReader = new BinaryReader(reader);

                    var version = binaryReader.ReadInt32();
                    if (version != 1)
                    {
                        ShowError($"Workspace version not supported !{Environment.NewLine}Name: {name}, Version: {version}");
                        return;
                    }

                    var memory = new MemoryStream();

                    memory.Fetch(reader);
                    var hosts = _screenManager.LoadWorkspace(memory);

                    _mainWindow.SetPositionAndSize(binaryReader.ReadRectangleSettings());

                    memory.Fetch(reader);
                    _mainWindow.LoadWorkspace(memory, id => hosts[id]);
                }
            }
            catch (Exception e)
            {
                ShowError($"Workspace {name} corrupted !", e);
            }
        }

        private string GetFolder() => Folder ?? @".\";

        private string GetFile(string name, bool createFolder = false)
        {
            var folder = GetFolder();
            if (createFolder && !Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return Path.Combine(folder, $"{name}.workspace");
        }

        private string GetCurrentWorkspace()
        {
            var file = GetCurrentFile();
            return File.Exists(file) ? File.ReadAllText(file) : "default";
        }

        private string GetCurrentFile() => Path.Combine(GetFolder(), ".current");

        private void ShowError(string message, Exception e = null)
        {
            _viewBox.Show(message, "Workspace issue", ViewboxButtons.Ok, ViewboxImage.Error);
        }
    }
}
