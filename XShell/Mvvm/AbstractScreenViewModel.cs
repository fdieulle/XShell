using System;
using System.ComponentModel;

namespace XShell.Mvvm
{
    public class AbstractScreenViewModel : INotifyPropertyChanged, IScreen
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of IScreen

        public event Action TitleChanged;

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title == value) return;

                title = value;
                RaiseTitleChanged();
            }
        }

        #endregion

        protected void RaisePropertyChanged(string propertyName = null)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (e == null) return;
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void RaiseTitleChanged()
        {
            var handler = TitleChanged;
            if (handler != null)
                handler();
        }
    }
}
