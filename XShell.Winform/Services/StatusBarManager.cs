using System;
using System.Windows.Forms;

namespace XShell.Winform.Services
{
    public class StatusBarManager : IDisposable
    {
        private readonly ToolStripProgressBar _progressBar;
        private readonly ToolStripLabel _progressLabel;
        private readonly IBackgroundTaskManager _backgroundTaskManager;

        public StatusBarManager(
            ToolStripProgressBar progressBar,
            ToolStripLabel progressLabel,
            IBackgroundTaskManager backgroundTaskManager)
        {
            _progressBar = progressBar;
            _progressLabel = progressLabel;
            _backgroundTaskManager = backgroundTaskManager;

            _backgroundTaskManager.TaskStarted += OnBackgroundTaskStarted;
            _backgroundTaskManager.ReportStateChanged += OnBackgroundReportStateChanged;
            _backgroundTaskManager.TaskCompleted += OnBackgroundTaskCompleted;
        }

        private void OnBackgroundTaskStarted(IBackgroundTask task, object state)
        {
            _progressBar.Visible = true;
            _progressLabel.Visible = true;
            _progressBar.Style = task.IsIndeterminate ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
            if (state != null)
                _progressLabel.Text = state.ToString();
        }

        private void OnBackgroundReportStateChanged(double percent, object state)
        {
            _progressBar.Value = (int)percent;
            if (state != null)
                _progressLabel.Text = state.ToString();
        }

        private void OnBackgroundTaskCompleted(IBackgroundTask arg1, object arg2)
        {
            _progressBar.Visible = false;
            _progressLabel.Visible = false;
            _progressBar.Style = ProgressBarStyle.Blocks;
            _progressLabel.Text = null;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _backgroundTaskManager.TaskStarted -= OnBackgroundTaskStarted;
            _backgroundTaskManager.ReportStateChanged -= OnBackgroundReportStateChanged;
            _backgroundTaskManager.TaskCompleted -= OnBackgroundTaskCompleted;
        }

        #endregion
    }
}
