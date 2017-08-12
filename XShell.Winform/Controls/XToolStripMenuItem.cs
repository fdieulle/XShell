using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace XShell.Winform.Controls
{
    public class XToolStripMenuItem : ToolStripMenuItem, IMenuItem
    {
        #region Implementation of IMenuItem

        public string DisplayName
        {
            get { return Text; }
            set { Text = value; }
        }

        private string iconFilePath;
        public string IconFilePath
        {
            get { return iconFilePath; }
            set
            {
                iconFilePath = value;
                if (iconFilePath != null && File.Exists(iconFilePath))
                    Image = new Bitmap(iconFilePath);
                else Image = null;
            }
        }

        public Action Action { get; set; }

        public bool IsEnabled
        {
            get { return Enabled; }
            set { Enabled = value; }
        }

        public bool IsVisible
        {
            get { return Visible; }
            set { Visible = value; }
        }

        #endregion

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            var action = Action;
            if (action != null)
                action();
        }
    }
}
