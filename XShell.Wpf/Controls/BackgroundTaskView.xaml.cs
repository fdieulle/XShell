﻿using System.Windows.Controls;

namespace XShell.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for BackgroundTaskView.xaml
    /// </summary>
    public partial class BackgroundTaskView : UserControl
    {
        public bool IsIndeterminate
        {
            get => ProgressBar.IsIndeterminate;
            set => ProgressBar.IsIndeterminate = value;
        }

        public double Percent
        {
            get => ProgressBar.Value;
            set => ProgressBar.Value = value;
        }

        public string State
        {
            get => ProgressState.Text;
            set => ProgressState.Text = value;
        }

        public BackgroundTaskView()
        {
            InitializeComponent();
        }
    }
}
