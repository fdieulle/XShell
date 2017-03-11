using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace XShell.Demo.Wpf.Controls
{
    public class XMenuItem : MenuItem, IMenuItem
    {
        #region Implementation of IMenuItem

        public string DisplayName
        {
            get { return Header as string; }
            set { Header = value; }
        }

        private string iconFilePath;
        public string IconFilePath
        {
            get { return iconFilePath; }
            set
            {
                if (iconFilePath == value) return;
                iconFilePath = value;
                Icon = !string.IsNullOrEmpty(iconFilePath) ? new Image { Source = new BitmapImage(new Uri(iconFilePath)) } : null;
            }
        }

        public Action Action { get; set; }

        public new bool IsVisible
        {
            get { return Visibility == Visibility.Visible; }
            set { Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        #endregion

        protected override void OnClick()
        {
            base.OnClick();

            var action = Action;
            if (action != null)
                action();
        }
    }
}
