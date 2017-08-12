using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using XShell.Core;
using XShell.Winform.Binders;

namespace XShell.Winform
{
    public static class Extensions
    {
        /// <summary>
        /// Binds a command to a button.
        /// </summary>
        /// <returns>Returns a <see cref="IDisposable"/> instance, which has to be disposed to avoid memory leaks</returns>
        public static IDisposable Bind(this Button button, IRelayCommand command, Func<object> getParameter = null)
        {
            if (button == null || command == null) return AnonymousDisposable.Empty;

            return new ButtonBinder(button, command, getParameter);
        }

        /// <summary>
        /// Binds a <see cref="ICollectionSelector"/> to a <see cref="ComboBox"/>.
        /// </summary>
        /// <returns>Returns a <see cref="IDisposable"/> instance, which has to be disposed to avoid memory leaks</returns>
        public static IDisposable Bind(this ComboBox comboBox, ICollectionSelector selector)
        {
            if (comboBox == null || selector == null) return AnonymousDisposable.Empty;

            return new ComboBoxBinder(comboBox, selector);
        }

        /// <summary>
        /// Binds a <see cref="IList"/> instance to another <see cref="IList"/> instance. Usefull if the logic one implements <see cref="INotifyCollectionChanged"/>
        /// </summary>
        /// <returns>Returns a <see cref="IDisposable"/> instance, which has to be disposed to avoid memory leaks</returns>
        public static IDisposable Bind(this IList view, IEnumerable logic)
        {
            if (view == null || logic == null) return AnonymousDisposable.Empty;

            return new ListBinder(view, logic);
        }
    }
}
