namespace XShell
{
    public enum ViewBoxResult
    {
        None,
        Ok,
        Cancel,
        No,
        Yes,
    }

    public enum ViewboxButtons
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel
    }

    public enum ViewboxImage
    {
        None,
        Asterisk,
        Error,
        Exclamation,
        Warning,
        Information,
        Hand,
        Question,
        Stop
    }

    public interface IViewBox
    {
        ViewBoxResult Show(string text, string caption = null, ViewboxButtons buttons = ViewboxButtons.Ok, ViewboxImage image = ViewboxImage.None);

        string[] AskFiles(string filter = null, string initialFolder = null, string defaultExt = null, bool multiSelect = false);
    }

    public static class ViewBoxExtensions
    {
        public static string AskFile(this IViewBox viewBox, string filter = null, string initialFolder = null, string defaultExt = null)
        {
            var files = viewBox.AskFiles(filter, initialFolder, defaultExt);
            return files != null && files.Length > 0
                ? files[0] : null;
        }
    }
}
