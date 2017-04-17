using System;
using System.Windows.Forms;

namespace XShell.Demo.Winform.Services.Shell
{
    public class StatusBarManager : IDisposable
    {
        private readonly ToolStripProgressBar progressBar;
        private readonly ToolStripLabel progressLabel;
        private readonly IBackgroundTaskManager backgroundTaskManager;

        public StatusBarManager(
            ToolStripProgressBar progressBar,
            ToolStripLabel progressLabel,
            IBackgroundTaskManager backgroundTaskManager)
        {
            this.progressBar = progressBar;
            this.progressLabel = progressLabel;
            this.backgroundTaskManager = backgroundTaskManager;

            this.backgroundTaskManager.TaskStarted += OnBackgroundTaskStarted;
            this.backgroundTaskManager.ReportStateChanged += OnBackgroundReportStateChanged;
            this.backgroundTaskManager.TaskCompleted += OnBackgroundTaskCompleted;
        }

        private void OnBackgroundTaskStarted(IBackgroundTask task, object state)
        {
            this.progressBar.Visible = true;
            this.progressLabel.Visible = true;
            this.progressBar.Style = task.IsIndeterminate ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
            if (state != null)
                this.progressLabel.Text = state.ToString();
        }

        private void OnBackgroundReportStateChanged(double percent, object state)
        {
            this.progressBar.Value = (int)percent;
            if (state != null)
                this.progressLabel.Text = state.ToString();
        }

        private void OnBackgroundTaskCompleted(IBackgroundTask arg1, object arg2)
        {
            this.progressBar.Visible = false;
            this.progressLabel.Visible = false;
            this.progressBar.Style = ProgressBarStyle.Blocks;
            this.progressLabel.Text = null;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.backgroundTaskManager.TaskStarted -= OnBackgroundTaskStarted;
            this.backgroundTaskManager.ReportStateChanged -= OnBackgroundReportStateChanged;
            this.backgroundTaskManager.TaskCompleted -= OnBackgroundTaskCompleted;
        }

        #endregion
    }
}
