using System;

namespace XShell.Core
{
    public class AnonymousDisposable : IDisposable
    {
        public static readonly IDisposable Empty = new AnonymousDisposable(null);

        private Action _onDispose;

        public AnonymousDisposable(Action onDispose)
        {
            _onDispose = onDispose;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (_onDispose == null) return;
            
            _onDispose();
            _onDispose = null;
        }

        #endregion
    }
}
