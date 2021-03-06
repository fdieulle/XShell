﻿using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using XShell.Services;

namespace XShell.Winform.Controls
{
    public partial class MainForm : Form, IMainWindow<XDockContent>
    {
        public DockPanel MainDockPanel => mainDockPanel;

        public StatusStrip MainStatusBar => mainStatusBar;

        public ToolStripProgressBar StatusProgressBar => progressBar;
        public ToolStripStatusLabel StatusProgressLabel => progressLabel;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Overrides of Form

        protected override void OnClosing(CancelEventArgs e)
        {
            Terminating?.Invoke();
            base.OnClosing(e);
        }

        #endregion

        #region Implementation of IMainWindow

        public event Action Terminating;

        public RectangleSettings GetPositionAndSize()
        {
            return new RectangleSettings
            {
                Top = Top,
                Left = Left,
                Width = Width,
                Height = Height
            };
        }

        public void SetPositionAndSize(RectangleSettings rectangle)
        {
            Top = (int)rectangle.Top;
            Left = (int)rectangle.Left;
            Width = (int)rectangle.Width;
            Height = (int)rectangle.Height;
        }

        public void SaveWorkspace(Stream stream) 
            => MainDockPanel.SaveAsXml(stream, Encoding.Unicode);

        public void LoadWorkspace(Stream stream, Func<string, XDockContent> createContent) 
            => MainDockPanel.LoadFromXml(stream, id => createContent(id));

        #endregion
    }
}
