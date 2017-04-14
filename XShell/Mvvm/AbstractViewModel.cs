using System;
using System.ComponentModel;
using XShell.Services;

namespace XShell.Mvvm
{
    public class AbstractViewModel : AbstractLogic, INotifyPropertyChanged, IScreen
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of IScreen

        public event Action TitleChanged;

        private static readonly PropertyChangedEventArgs titlePropertyChanged = new PropertyChangedEventArgs("Title");
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (title == value) return;

                title = value;
                RaiseTitleChanged();
                RaisePropertyChanged(titlePropertyChanged);
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
