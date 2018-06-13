using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace XShell.Wpf.Controls
{
    public class XMenuItem : MenuItem, IMenuItem
    {
        #region Implementation of IMenuItem

        public string DisplayName
        {
            get { return Header as string; }
            set { Header = value; }
        }

        private string _iconFilePath;
        public string IconFilePath
        {
            get { return _iconFilePath; }
            set
            {
                if (_iconFilePath == value) return;
                _iconFilePath = value;
                Icon = !string.IsNullOrEmpty(_iconFilePath) ? new Image { Source = new BitmapImage(new Uri(_iconFilePath)) } : null;
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
