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

        public static void Close<TScreen>(this IScreenManager manager, string instanceId = null) where TScreen : IScreen
        {
            manager.Close(typeof(TScreen), instanceId);
        }

        public static void CloseAll<TScreen>(this IScreenManager manager) where TScreen : IScreen
        {
            manager.CloseAll(typeof(TScreen));
        }

        #endregion

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
    }
}
