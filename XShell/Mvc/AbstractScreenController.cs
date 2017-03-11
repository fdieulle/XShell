using System;

namespace XShell.Mvc
{
    public class AbstractScreenController : IScreen
    {
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

        protected void RaiseTitleChanged()
        {
            var handler = TitleChanged;
            if (handler != null)
                handler();
        }
    }
}
