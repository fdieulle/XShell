using System;
using XShell.Services;

namespace XShell.Mvc
{
    public class AbstractController : AbstractLogic, IScreen
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
