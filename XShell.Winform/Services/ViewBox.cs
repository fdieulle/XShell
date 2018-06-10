using System.Windows.Forms;

namespace XShell.Winform.Services
{
    public class ViewBox : IViewBox
    {
        private readonly Form mainForm;

        public ViewBox(Form mainForm)
        {
            this.mainForm = mainForm;
        }


        public ViewBoxResult Show(string text, string caption = null, ViewboxButtons buttons = ViewboxButtons.Ok, ViewboxImage image = ViewboxImage.None)
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


        private static MessageBoxButtons ToMbb(ViewboxButtons b)
        {
            switch (b)
            {
                case ViewboxButtons.Ok:
                default:
                    return MessageBoxButtons.OK;
                case ViewboxButtons.OkCancel:
                    return MessageBoxButtons.OKCancel;
                case ViewboxButtons.YesNo:
                    return MessageBoxButtons.YesNo;
                case ViewboxButtons.YesNoCancel:
                    return MessageBoxButtons.YesNoCancel;
            }
        }

        private static MessageBoxIcon ToMbi(ViewboxImage i)
        {
            switch (i)
            {
                case ViewboxImage.Asterisk:
                    return MessageBoxIcon.Asterisk;
                case ViewboxImage.Error:
                    return MessageBoxIcon.Error;
                case ViewboxImage.Exclamation:
                    return MessageBoxIcon.Exclamation;
                case ViewboxImage.Hand:
                    return MessageBoxIcon.Hand;
                case ViewboxImage.Information:
                    return MessageBoxIcon.Information;
                case ViewboxImage.Question:
                    return MessageBoxIcon.Question;
                case ViewboxImage.Stop:
                    return MessageBoxIcon.Stop;
                case ViewboxImage.Warning:
                    return MessageBoxIcon.Warning;
                default:
                    return MessageBoxIcon.None;
            }
        }

        private static ViewBoxResult ToVbr(DialogResult r)
        {
            switch (r)
            {
                case DialogResult.OK:
                    return ViewBoxResult.Ok;
                case DialogResult.No:
                    return ViewBoxResult.No;
                case DialogResult.Cancel:
                    return ViewBoxResult.Cancel;
                case DialogResult.Yes:
                    return ViewBoxResult.Yes;
                default:
                    return ViewBoxResult.None;
            }
        }
    }
}
