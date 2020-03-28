using System.Windows;
using Microsoft.Win32;

namespace XShell.Wpf.Services
{
    public class ViewBox : IViewBox
    {
        public ViewBoxResult Show(
            string text, string caption = null, 
            ViewboxButtons buttons = ViewboxButtons.Ok, 
            ViewboxImage image = ViewboxImage.None)
        {
            return ToVbr(MessageBox.Show(text, caption, ToMbb(buttons), ToMbi(image)));
        }

        public string[] AskFiles(string filter = null, string initialFolder = null, string defaultExt = null, bool multiSelect = false)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = multiSelect
            };

            if (filter != null)
                dialog.Filter = filter;
            if (initialFolder != null)
                dialog.InitialDirectory = initialFolder;
            if (defaultExt != null)
                dialog.DefaultExt = defaultExt;

            if (dialog.ShowDialog() ?? false)
                return dialog.FileNames;
            return null;
        }

        #region Helpers

        private static MessageBoxButton ToMbb(ViewboxButtons b)
        {
            switch(b)
            {
                default:
                    return MessageBoxButton.OK;
                case ViewboxButtons.OkCancel:
                    return MessageBoxButton.OKCancel;
                case ViewboxButtons.YesNo:
                    return MessageBoxButton.YesNo;
                case ViewboxButtons.YesNoCancel:
                    return MessageBoxButton.YesNoCancel;
            }
        }

        private static MessageBoxImage ToMbi(ViewboxImage i)
        {
            switch(i)
            {
                case ViewboxImage.Asterisk:
                    return MessageBoxImage.Asterisk;
                case ViewboxImage.Error:
                    return MessageBoxImage.Error;
                case ViewboxImage.Exclamation:
                    return MessageBoxImage.Exclamation;
                case ViewboxImage.Hand:
                    return MessageBoxImage.Hand;
                case ViewboxImage.Information:
                    return MessageBoxImage.Information;
                case ViewboxImage.Question:
                    return MessageBoxImage.Question;
                case ViewboxImage.Stop:
                    return MessageBoxImage.Stop;
                case ViewboxImage.Warning:
                    return MessageBoxImage.Warning;
                default:
                    return MessageBoxImage.None;
            }
        }

        private static ViewBoxResult ToVbr(MessageBoxResult r)
        {
            switch (r)
            {
                case MessageBoxResult.OK:
                    return ViewBoxResult.Ok;
                case MessageBoxResult.No:
                    return ViewBoxResult.No;
                case MessageBoxResult.Cancel:
                    return ViewBoxResult.Cancel;
                case MessageBoxResult.Yes:
                    return ViewBoxResult.Yes;
                default:
                    return ViewBoxResult.None;
            }
        }

        #endregion // Helpers
    }
}
