using System;

namespace XShell.Core
{
    public class AnonymousDisposable : IDisposable
    {
        public static readonly IDisposable Empty = new AnonymousDisposable(null);

        private Action onDispose;

        public AnonymousDisposable(Action onDispose)
        {
            this.onDispose = onDispose;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (this.onDispose == null) return;
            
            this.onDispose();
            this.onDispose = null;
        }

        #endregion
    }
}
