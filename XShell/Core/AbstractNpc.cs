using System.ComponentModel;

namespace XShell.Core
{
    public abstract class AbstractNpc : INotifyPropertyChanged
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName = null)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (e == null) return;
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}
