using System.Windows.Controls;

namespace XShell.Demo.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for BackgroundTaskView.xaml
    /// </summary>
    public partial class BackgroundTaskView : UserControl
    {
        public bool IsIndeterminate
        {
            get { return this.ProgressBar.IsIndeterminate; }
            set { this.ProgressBar.IsIndeterminate = value; }
        }

        public double Percent
        {
            get { return this.ProgressBar.Value; }
            set { this.ProgressBar.Value = value; }
        }

        public string State
        {
            get { return this.ProgressState.Text; }
            set { this.ProgressState.Text = value; }
        }

        public BackgroundTaskView()
        {
            InitializeComponent();
        }
    }
}
