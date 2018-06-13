using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows.Forms;
using XShell.Core;

namespace XShell.Winform.Binders
{
    public static class Binder
    {
        private static readonly ToolTip globalTootlTip = new ToolTip();

        /// <summary>
        /// Binds a <see cref="IRelayCommand"/> to a <see cref="Button"/>.
        /// </summary>
        /// <param name="button">Button to apply binding.</param>
        /// <param name="command">Command to bind.</param>
        /// <param name="getParameter">Function which allows to get command parameter. Null by default.</param>
        /// <param name="bindName">Defines if the command name will be bind with <see cref="Button.Text"/>. False by default.</param>
        /// <param name="toolTip">Defines a tooltip message. Null by default</param>
        /// <returns>Returns an <see cref="IDisposable"/> instance. Dispose it to ends binding.</returns>
        public static IDisposable Bind(this Button button, 
            IRelayCommand command, Func<object> getParameter = null,
            bool bindName = false, string toolTip = null)
        {
            return new ButtonBinder(button, command, getParameter, bindName, globalTootlTip, toolTip);
        }

        /// <summary>
        /// Binds a data context property to a <see cref="Label.Text"/>.
        /// </summary>
        /// <param name="label"><see cref="Label"/> to apply binding.</param>
        /// <param name="dataContext">Data context which provides the property name.</param>
        /// <param name="propertyName">Property name from data context.</param>
        /// <returns>Returns an <see cref="IDisposable"/> instance. Dispose it to ends binding.</returns>
        public static IDisposable Bind(this Label label, object dataContext, string propertyName)
        {
            return new LabelBinder(label, dataContext, propertyName);
        }

        /// <summary>
        /// Binds a data context property to a <see cref="TextBox.Text"/>.
        /// </summary>
        /// <param name="textBox"><see cref="TextBox"/> to apply binding.</param>
        /// <param name="dataContext">Data context which provides the property name.</param>
        /// <param name="propertyName">Property name from data context.</param>
        /// <param name="allowDragDrop">Defines if the <see cref="TextBox"/> allows drag and drop behavior.</param>
        /// <returns>Returns an <see cref="IDisposable"/> instance. Dispose it to ends binding.</returns>
        public static IDisposable Bind(this TextBox textBox, object dataContext, string propertyName, bool allowDragDrop = false)
        {
            return new TextBoxBinder(textBox, dataContext, propertyName, allowDragDrop);
        }

        /// <summary>
        /// Binds a <see cref="IList"/> instance to another <see cref="IEnumerable"/> source.
        /// Usefull if the source implements <see cref="INotifyCollectionChanged"/> interface.
        /// </summary>
        /// <param name="view">view</param>
        /// <param name="source">source</param>
        /// <returns>Returns an <see cref="IDisposable"/> instance. Dispose it to ends binding.</returns>
        public static IDisposable Bind(this IList view, IEnumerable source)
        {
            return view == null ? AnonymousDisposable.Empty : new ListBinder(view, source);
        }

        /// <summary>
        /// Binds a <see cref="ICollectionSelector"/> to a <see cref="ComboBox"/>.
        /// </summary>
        /// <param name="comboBox"><see cref="TextBox"/> to apply binding.</param>
        /// <param name="selector"><see cref="ICollectionSelector"/> to bind.</param>
        /// <returns>Returns an <see cref="IDisposable"/> instance. Dispose it to ends binding.</returns>
        public static IDisposable Bind(this ComboBox comboBox, ICollectionSelector selector)
        {
            if (comboBox == null) return AnonymousDisposable.Empty;

            return new ComboBoxBinder(comboBox, selector);
        }

        #region Reflection 

        private static readonly Dictionary<Type, Dictionary<string, PropertyAccessor>>  typesMapping = new Dictionary<Type, Dictionary<string, PropertyAccessor>>();

        public static Func<object, TProperty> BuildPropertyGetter<TProperty>(this object dataContext, string propertyName)
        {
            return dataContext.GetType().BuildPropertyGetter<TProperty>(propertyName);
        }

        public static Func<object, TProperty> BuildPropertyGetter<TProperty>(this Type type, string propertyName)
        {
            if (type == null || string.IsNullOrEmpty(propertyName)) return null;

            if (!typesMapping.TryGetValue(type, out var properties))
                typesMapping.Add(type, properties = new Dictionary<string, PropertyAccessor>());

            if(!properties.TryGetValue(propertyName, out var property))
                properties.Add(propertyName, property = new PropertyAccessor(propertyName));

            if (property.Getter == null)
                property.Getter = (Delegate)createGetterMethod.MakeGenericMethod(type, typeof(TProperty))
                    .Invoke(null, new object[]{ propertyName });

            return (Func<object, TProperty>)property.Getter;
        }

        private static readonly MethodInfo createGetterMethod = MethodBase.GetCurrentMethod()
            .DeclaringType?.GetMethod("CreateGetter", BindingFlags.NonPublic | BindingFlags.Static);
        // ReSharper disable once UnusedMember.Local
        private static Func<object, TProperty> CreateGetter<TType, TProperty>(string propertyName)
        {
            var property = typeof(TType).GetProperty(propertyName);
            if (property == null) return p => default(TProperty);

            var getMethod = property.GetGetMethod();
            if(getMethod == null) return p => default(TProperty);

            var getPropertyValue = (Func<TType, TProperty>)Delegate.CreateDelegate(typeof(Func<TType, TProperty>), getMethod);
            return p => getPropertyValue((TType) p);
        }

        public static Action<object, TProperty> BuildPropertySetter<TProperty>(this object dataContext, string propertyName)
        {
            return dataContext.GetType().BuildPropertySetter<TProperty>(propertyName);
        }

        public static Action<object, TProperty> BuildPropertySetter<TProperty>(this Type type, string propertyName)
        {
            if (type == null || string.IsNullOrEmpty(propertyName)) return null;

            if (!typesMapping.TryGetValue(type, out var properties))
                typesMapping.Add(type, properties = new Dictionary<string, PropertyAccessor>());

            if (!properties.TryGetValue(propertyName, out var property))
                properties.Add(propertyName, property = new PropertyAccessor(propertyName));

            if (property.Setter == null)
                property.Setter = (Delegate)createSetterMethod.MakeGenericMethod(type, typeof(TProperty))
                    .Invoke(null, new object[] { propertyName });

            return (Action<object, TProperty>)property.Setter;
        }

        private static readonly MethodInfo createSetterMethod = MethodBase.GetCurrentMethod()
            .DeclaringType?.GetMethod("CreateSetter", BindingFlags.NonPublic | BindingFlags.Static);
        // ReSharper disable once UnusedMember.Local
        private static Action<object, TProperty> CreateSetter<TType, TProperty>(string propertyName)
        {
            var property = typeof(TType).GetProperty(propertyName);
            if (property == null) return (p, v) => { };

            var setMethod = property.GetSetMethod();
            if (setMethod == null) return (p, v) => { };

            var setPropertyValue = (Action<TType, TProperty>)Delegate.CreateDelegate(typeof(Action<TType, TProperty>), setMethod);
            return (p, v) => setPropertyValue((TType)p, v);
        }

        private class PropertyAccessor
        {
            private readonly string _name;

            public Delegate Getter { get; set; }

            public Delegate Setter { get; set; }

            public PropertyAccessor(string name)
            {
                _name = name;
            }

            #region Overrides of Object

            public override string ToString()
            {
                return _name;
            }

            #endregion
        }

        #endregion
    }
}
