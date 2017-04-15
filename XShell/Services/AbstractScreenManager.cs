using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XShell.Core;

namespace XShell.Services
{
    public abstract class AbstractScreenManager<TBaseView> : IScreenManager, IScreenContainer
        where TBaseView : class
    {
        private readonly Action<Type, Type> register;
        private readonly Func<Type, object> resolve;
        private readonly IMenuManager menuManager;
        private readonly IPersistenceService persistenceService;

        private readonly Dictionary<Type, ScreenFactory> factories = new Dictionary<Type, ScreenFactory>();
        private readonly Dictionary<NamedType, ScreenHost> screens = new Dictionary<NamedType, ScreenHost>();

        protected AbstractScreenManager(Action<Type, Type> register, Func<Type, object> resolve,
            IMenuManager menuManager = null, IPersistenceService persistenceService = null)
        {
            this.register = register;
            this.resolve = resolve;
            this.menuManager = menuManager;
            this.persistenceService = persistenceService;
        }

        #region Implementation of IScreenContainer

        public void Register(Type idType, Type viewType, Type logicType)
        {
            CheckParameters(idType, viewType, logicType);

            var popupAttribute = idType.GetCustomAttribute<PopupAttribute>(true);
            factories[idType] = new ScreenFactory(idType, viewType, logicType, register, resolve, popupAttribute);

            if (menuManager != null)
            {
                var attribute = idType.GetCustomAttributes(typeof(ScreenMenuItemAttribute), true)
                      .OfType<ScreenMenuItemAttribute>()
                      .FirstOrDefault();
                if (attribute != null)
                {
                    var action = attribute.IsPopup
                        ? new Action(() => Popup(idType))
                        : () => Display(idType);
                    menuManager.Add(attribute.Path, action, attribute.DisplayName, attribute.IconFilePath);
                }
            }
        }

        #endregion

        #region Implementation of IScreenManager

        public void Display(Type idType, string instanceId = null, object parameter = null)
        {
            Show(idType, instanceId, parameter, false);
        }

        public void Popup(Type idType, string instanceId = null, object parameter = null)
        {
            Show(idType, instanceId, parameter, true);
        }

        public void SetParameter(Type idType, string instanceId, object parameter)
        {
            var key = new NamedType(idType, instanceId);
            ScreenHost host;
            if (screens.TryGetValue(key, out host))
                host.SetParameter(parameter);
        }

        public object GetParameter(Type idType, string instanceId)
        {
            var key = new NamedType(idType, instanceId);
            ScreenHost host;
            return screens.TryGetValue(key, out host) ? host.GetParameter() : null;
        }

        public void Close(Type idType, string instanceId = null)
        {
            var key = new NamedType(idType, instanceId);
            ScreenHost host;
            if (screens.TryGetValue(key, out host))
                host.Close();
        }

        public void CloseAll(Type idType)
        {
            foreach (var key in screens.Select(p => p.Key).Where(p => p.Type == idType).ToArray())
                Close(key.Type, key.Name);
        }

        #endregion

        private static void CheckParameters(Type idType, Type viewType, Type logicType)
        {
            if(idType == null)
                throw new ArgumentNullException("idType");
            if (viewType == null)
                throw new ArgumentNullException("viewType");
            if (logicType == null)
                throw new ArgumentNullException("logicType");

            if (!idType.InheritsFrom(typeof(IScreen)))
                throw new ArgumentException("The idType which allows you to resolve and display screen has to implement IScreen interface, but was: " + idType.FullName, "idType");
            if (!viewType.InheritsFrom(typeof(TBaseView)))
                throw new ArgumentException("The viewType which allows you to resolve and display screen has to inherit from " + typeof(TBaseView).FullName + ", but was: " + viewType.FullName, "viewType");
            if (!logicType.InheritsFrom(idType))
                throw new ArgumentException("The logicType which will resolve and display screen has to implement " + idType.FullName + " interface, but was: " + logicType.FullName, "logicType");
        }

        private void Show(Type idType, string instanceId, object parameter, bool isPopup)
        {
            var key = new NamedType(idType, instanceId);
            ScreenHost host;
            if (screens.TryGetValue(key, out host))
            {
                host.BringToFront(isPopup);
                return;
            }

            ScreenFactory factory;
            if (!factories.TryGetValue(key.Type, out factory)) return;

            IScreen logic;
            TBaseView view;
            Exception e;
            if (!factory.TryCreate(instanceId, parameter, out logic, out view, out e))
            {
                OnException(string.Format("Unable to create screen: {0}", key), e);
                return;
            }

            host = new ScreenHost(key, view, logic, CreateScreen, CreatePopup, factory.PopupAttribute);
            screens.Add(key, host);
            host.Closed += OnHostClosed;

            var setupable = logic as IInternalScreen;
            if(setupable != null)
                setupable.Setup(host.Close);

            if (!host.Restore(persistenceService, out e))
                OnException(string.Format("Unable to restore screen: {0}", key), e);

            host.Show(isPopup);
        }

        private void OnHostClosed(ScreenHost host)
        {
            host.Closed -= OnHostClosed;
            screens.Remove(host.Key);
            Exception e;
            if(!host.Persist(persistenceService, out e))
                OnException(string.Format("Unable to restore screen: {0}", host.Key), e);
            host.Dispose();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            foreach (var key in screens.Select(p => p.Key).ToArray())
                Close(key.Type, key.Name);
            factories.Clear();
        }

        #endregion

        protected abstract IScreenHost CreateScreen(TBaseView view);

        protected abstract IScreenHost CreatePopup(TBaseView view, PopupAttribute popupAttribute);

        protected abstract void OnException(string message, Exception e);

        #region Nested type

        private class ScreenFactory
        {
            private readonly Type idType;
            private readonly Func<Type, object> resolve;
            private readonly Func<IScreen, TBaseView> factory;

            public PopupAttribute PopupAttribute { get; private set; }
            
            public ScreenFactory(Type idType, Type viewType, Type logicType, Action<Type, Type> register, Func<Type, object> resolve, PopupAttribute popupAttribute)
            {
                this.idType = idType;
                this.resolve = resolve;
                this.PopupAttribute = popupAttribute;

                factory = idType != viewType
                    ? viewType.CreateFactory<TBaseView>(idType)
                    : (p => (TBaseView)p);

                register(idType, logicType);
            }

            public bool TryCreate(string instanceId, object parameter, out IScreen logic, out TBaseView view, out Exception ex)
            {
                ex = null;
                try
                {
                    logic = resolve(idType) as IScreen;
                    var setupable = logic as IInternalScreen;
                    if (setupable != null)
                    {
                        setupable.Setup(instanceId);
                        setupable.Parameter = parameter;
                    }

                    view = factory(logic);
                    return true;
                }
                catch (Exception e)
                {
                    logic = null;
                    view = null;
                    ex = e;
                    return false;
                }
            }
        }

        protected class ScreenHost : IDisposable
        {
            private readonly NamedType key;
            private readonly TBaseView view;
            private readonly IScreen screen;
            private readonly Func<TBaseView, IScreenHost> createScreen;
            private readonly Func<TBaseView, PopupAttribute, IScreenHost> createPopup;
            private readonly PopupAttribute popupAttribute;

            private bool isInPopup;
            private IScreenHost host;

            public NamedType Key { get { return key; } }

            public ScreenHost(NamedType key,
                TBaseView view,
                IScreen screen,
                Func<TBaseView, IScreenHost> createScreen,
                Func<TBaseView, PopupAttribute, IScreenHost> createPopup,
                PopupAttribute popupAttribute)
            {
                this.key = key;
                this.view = view;
                this.screen = screen;
                this.createScreen = createScreen;
                this.createPopup = createPopup;
                this.popupAttribute = popupAttribute;
                this.screen.TitleChanged += OnTitleChanged;
            }

            public event Action<ScreenHost> Closed;

            public void Close()
            {
                this.host.Close();
            }

            public void Show(bool isPopup)
            {
                host = isPopup ? createPopup(view, popupAttribute) : createScreen(view);

                if (screen != null)
                    host.Title = screen.Title;

                host.ScreenClosed += OnScreenClosed;
                isInPopup = isPopup;
            }

            public void BringToFront(bool isPopup)
            {
                if (isPopup == isInPopup)
                    host.BringToFront();
                else
                {
                    host.ScreenClosed -= OnScreenClosed;
                    host.Close();
                    Show(isPopup);
                }
            }

            private void OnScreenClosed(IScreenHost screenHost)
            {
                var handler = Closed;
                if (handler != null)
                    handler(this);
            }

            public void SetParameter(object parameter)
            {
                var cast = screen as IInternalScreen;
                if (cast != null)
                    cast.Parameter = parameter;
            }

            public object GetParameter()
            {
                var cast = screen as IInternalScreen;
                return cast != null ? cast.Parameter : null;
            }

            #region Persistency

            public bool Restore(IPersistenceService svc, out Exception ex)
            {
                try
                {
                    ex = null;
                    if (svc == null) return true;

                    var persistable = view as IPersistable;
                    if (persistable != null)
                        svc.Restore(GetViewName(), view as IPersistable);
                    
                    if (ReferenceEquals(screen, view)) return true; // In case of there is no screen type defined

                    // ReSharper disable SuspiciousTypeConversion.Global
                    persistable = screen as IPersistable;
                    // ReSharper restore SuspiciousTypeConversion.Global
                    if (persistable != null)
                        svc.Restore(GetLogicName(), persistable);
                    return true;
                }
                catch (Exception e)
                {
                    ex = e;
                    return false;
                }
            }

            public bool Persist(IPersistenceService svc, out Exception ex)
            {
                try
                {
                    ex = null;
                    if (svc == null) return true;

                    var persistable = view as IPersistable;
                    if (persistable != null)
                        svc.Persist(GetViewName(), persistable);
                    if (ReferenceEquals(screen, view)) return true; // In case of there is no screen type defined

                    // ReSharper disable SuspiciousTypeConversion.Global
                    persistable = screen as IPersistable;
                    // ReSharper restore SuspiciousTypeConversion.Global
                    if (persistable != null)
                        svc.Persist(GetLogicName(), persistable);
                    return true;
                }
                catch (Exception e)
                {
                    ex = e;
                    return false;
                }
            }

            private string GetViewName()
            {
                return string.Format("{0}_{1}_View", key.Type, key.Name);
            }

            private string GetLogicName()
            {
                return string.Format("{0}_{1}_Logic", key.Type, key.Name);
            }

            #endregion

            #region Manage screen events

            private void OnTitleChanged()
            {
                host.Title = screen.Title;
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                screen.TitleChanged -= OnTitleChanged;
                host.ScreenClosed -= OnScreenClosed;
                host = null;

                var disposable = screen as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            #endregion

            public override string ToString()
            {
                return string.Format("View: {0}, Logic: {1}", view, screen);
            }
        }

        #endregion
    }
}