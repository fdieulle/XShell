using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace XShell
{
    public static class Extensions
    {
        #region IScreenContainer extensions

        public static void Register<TInterface, TView, TLogic>(this IScreenContainer container)
            where TInterface : IScreen
            where TLogic : TInterface
        {
            container.Register(typeof(TInterface), typeof(TView), typeof(TLogic));
        }

        public static void Register<TView, TLogic>(this IScreenContainer container)
            where TLogic : IScreen
        {
            container.Register(typeof(TLogic), typeof(TView), typeof(TLogic));
        }

        public static void Register<TView>(this IScreenContainer container)
            where TView : IScreen
        {
            container.Register(typeof(TView), typeof(TView), typeof(TView));
        }

        #endregion

        #region IScreenContainer extensions

        public static void Display<TScreen>(this IScreenManager manager, string instanceId = null, object parameter = null) where TScreen : IScreen
        {
            manager.Display(typeof(TScreen), instanceId);
        }

        public static void Popup<TScreen>(this IScreenManager manager, string instanceId = null, object parameter = null) where TScreen : IScreen
        {
            manager.Popup(typeof(TScreen), instanceId);
        }

        public static void SetParameter<TScreen>(this IScreenManager manager, object parameter) where TScreen : IScreen
        {
            manager.SetParameter(typeof(TScreen), null, parameter);
        }

        public static void SetParameter<TScreen>(this IScreenManager manager, string instanceId, object parameter) where TScreen : IScreen
        {
            manager.SetParameter(typeof(TScreen), instanceId, parameter);
        }

        public static object GetParameter<TScreen>(this IScreenManager manager, string instanceId = null) where TScreen : IScreen
        {
            return manager.GetParameter(typeof(TScreen), instanceId);
        }

        public static void Close<TScreen>(this IScreenManager manager, string instanceId = null) where TScreen : IScreen
        {
            manager.Close(typeof(TScreen), instanceId);
        }

        public static void CloseAll<TScreen>(this IScreenManager manager) where TScreen : IScreen
        {
            manager.CloseAll(typeof(TScreen));
        }

        #endregion

        #region IBackgroundTaskManager extensions

        /// <summary>
        /// Run asynchronously a task in another thread. You can report at any time the state of your background task 
        /// through the <see cref="IBackgroundTask"/> interface given in onWork function parameter. 
        /// The report callback is send in the main ui thread.
        /// </summary>
        /// <typeparam name="TResult">Background work result type.</typeparam>
        /// <param name="manager"><see cref="IBackgroundTaskManager"/> which is extended.</param>
        /// <param name="onWork">Function which is executed asyncronously. The parameter allows you to report the state of the background work.</param>
        /// <param name="onCompleted">Callback called in the main ui thread when the work is finished. We provide the result as parameter.</param>
        public static void Dispatch<TResult>(this IBackgroundTaskManager manager, Func<IBackgroundTask, TResult> onWork, Action<TResult> onCompleted = null)
        {
            if (onWork == null) return;

            manager.Dispatch(
                (t, s) => onWork(t),
                onCompleted != null ? (r, s) => onCompleted(r) : (Action<TResult, object>) null);
        }

        /// <summary>
        /// Run asynchronously a task in another thread. You can report at any time the state of your background task 
        /// through the <see cref="IBackgroundTask"/> interface given in onWork function parameter. 
        /// The report callback is send in the main ui thread.
        /// </summary>
        /// <param name="manager"><see cref="IBackgroundTaskManager"/> which is extended.</param>
        /// <param name="onWork">Function which is executed asyncronously. The 1st parameter allows you to report the state of the background work. The 2nd parameter is the state given on the Dispatch method.</param>
        /// <param name="onCompleted">Callback called in the main ui thread when the work is finished. We provide the state as parameters.</param>
        /// <param name="state">User instance state forward on the onWork and onComplete callbacks.</param>
        public static void Dispatch(this IBackgroundTaskManager manager, Action<IBackgroundTask, object> onWork, Action<object> onCompleted = null, object state = null)
        {
            if (onWork == null) return;

            manager.Dispatch(
                (t, s) => 
                { 
                    onWork(t, s);
                    return null;
                },
                onCompleted != null ? (r, s) => onCompleted(s) : (Action<object, object>)null);
        }

        /// <summary>
        /// Run asynchronously a task in another thread. You can report at any time the state of your background task 
        /// through the <see cref="IBackgroundTask"/> interface given in onWork function parameter. 
        /// The report callback is send in the main ui thread.
        /// </summary>
        /// <param name="manager"><see cref="IBackgroundTaskManager"/> which is extended.</param>
        /// <param name="onWork">Function which is executed asyncronously. The 1st parameter allows you to report the state of the background work.</param>
        /// <param name="onCompleted">Callback called in the main ui thread when the work is finished.</param>
        public static void Dispatch(this IBackgroundTaskManager manager, Action<IBackgroundTask> onWork, Action onCompleted = null)
        {
            if (onWork == null) return;

            manager.Dispatch(
                (t, s) =>
                {
                    onWork(t);
                    return null;
                },
                onCompleted != null ? (r, s) => onCompleted() : (Action<object, object>)null);
        }

        /// <summary>
        /// Run asynchronously a task in another thread with an indeterminate time of work. 
        /// </summary>
        /// <typeparam name="TResult">Background work result type.</typeparam>
        /// <param name="manager"><see cref="IBackgroundTaskManager"/> which is extended.</param>
        /// <param name="onWork">Function which is executed asyncronously</param>
        /// <param name="onCompleted">Callback called in the main ui thread when the work is finished. We provide the result as parameter.</param>
        public static void Dispatch<TResult>(this IBackgroundTaskManager manager, Func<TResult> onWork, Action<TResult> onCompleted = null)
        {
            if (onWork == null) return;

            manager.Dispatch(
                (t, s) => onWork(),
                onCompleted != null ? (r, s) => onCompleted(r) : (Action<TResult, object>)null);
        }

        /// <summary>
        /// Run asynchronously a task in another thread with an indeterminate time of work. 
        /// </summary>
        /// <param name="manager"><see cref="IBackgroundTaskManager"/> which is extended.</param>
        /// <param name="onWork">Function which is executed asyncronously. The 1st parameter is the state given on the Dispatch method.</param>
        /// <param name="onCompleted">Callback called in the main ui thread when the work is finished. We provide the state as parameters.</param>
        /// <param name="state">User instance state forward on the onWork and onComplete callbacks.</param>
        public static void Dispatch(this IBackgroundTaskManager manager, Action<object> onWork, Action<object> onCompleted = null, object state = null)
        {
            if (onWork == null) return;

            manager.Dispatch(
                (t, s) =>
                {
                    onWork(s);
                    return null;
                },
                onCompleted != null ? (r, s) => onCompleted(s) : (Action<object, object>)null);
        }

        /// <summary>
        /// Run asynchronously a task in another thread with an indeterminate time of work. 
        /// </summary>
        /// <param name="manager"><see cref="IBackgroundTaskManager"/> which is extended.</param>
        /// <param name="onWork">Function which is executed asyncronously.</param>
        /// <param name="onCompleted">Callback called in the main ui thread when the work is finished.</param>
        public static void Dispatch(this IBackgroundTaskManager manager, Action onWork, Action onCompleted = null)
        {
            if (onWork == null) return;

            manager.Dispatch(
                (t, s) =>
                {
                    onWork();
                    return null;
                },
                onCompleted != null ? (r, s) => onCompleted() : (Action<object, object>)null);
        }

        #endregion

        #region Tools

        public static bool InheritsFrom(this Type type, Type baseType)
        {
            if (type == null || baseType == null) return false;

            if (baseType.IsInterface)
                return type.GetInterfaces().Any(p => p == baseType);

            while (type != null)
            {
                if (type == baseType) return true;
                type = type.BaseType;
            }

            return false;
        }

        public static void AppendException(this StringBuilder sb, Exception e)
        {
            while (e != null)
            {
                sb.AppendLine();
                sb.Append("[Message] ");
                sb.Append(e.Message);
                sb.AppendLine();
                sb.Append("[Source] ");
                sb.Append(e.Source);
                sb.AppendLine();
                sb.Append("[StackTrace] ");
                sb.Append(e.StackTrace);
                sb.AppendLine();
                e = e.InnerException;
            }
        }

        public static Func<IScreen, TBaseView> CreateFactory<TBaseView>(this Type viewType, Type idType)
        {
            var ctors = viewType.GetConstructors();
            foreach (var ctor in ctors)
            {
                var parameters = ctor.GetParameters();
                if (parameters.Length != 1 && !idType.InheritsFrom(parameters[0].ParameterType)) continue;

                var parameterType = parameters[0].ParameterType;
                return (Func<IScreen, TBaseView>)createFactoryMethod.MakeGenericMethod(typeof(IScreen), typeof(TBaseView), parameterType, viewType)
                                   .Invoke(null, new object[] { ctor });
            }

            throw new ArgumentException("Invalid view type. It should has a ctor to inject the logic instance as follow: public " + viewType.Name + "(" + idType.Name + " logic) {...}", "viewType");
        }

        private static readonly MethodInfo createFactoryMethod = MethodBase.GetCurrentMethod()
            .DeclaringType.GetMethod("CreateFactory", BindingFlags.NonPublic | BindingFlags.Static);
        // ReSharper disable UnusedMember.Local
        private static Func<TBaseParameter, TBaseObject> CreateFactory<TBaseParameter, TBaseObject, TParameter, TObject>(ConstructorInfo ctor)
            // ReSharper restore UnusedMember.Local
            where TParameter : TBaseParameter
            where TObject : TBaseObject
        {
            var dynamic = new DynamicMethod(string.Empty, typeof(TObject), new[] { typeof(TParameter) }, typeof(TObject));
            var il = dynamic.GetILGenerator();

            il.DeclareLocal(typeof(TObject));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Newobj, ctor);
            il.Emit(OpCodes.Ret);

            var func = (Func<TParameter, TObject>)dynamic.CreateDelegate(typeof(Func<TParameter, TObject>));
            return p => func((TParameter)p);
        }

        #endregion
    }
}
