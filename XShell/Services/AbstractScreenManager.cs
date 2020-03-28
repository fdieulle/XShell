using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using XShell.Core;

namespace XShell.Services
{
    public abstract class AbstractScreenManager<TBaseView> : IScreenManager, IScreenContainer
        where TBaseView : class
    {
        private readonly Action<Type, Type> _register;
        private readonly Func<Type, object> _resolve;
        private readonly IMenuManager _menuManager;
        private readonly IPersistenceService _persistenceService;

        private readonly Dictionary<Type, ScreenFactory> _factories = new Dictionary<Type, ScreenFactory>();
        private readonly Dictionary<NamedType, ScreenHost> _screens = new Dictionary<NamedType, ScreenHost>();

        protected AbstractScreenManager(Action<Type, Type> register, Func<Type, object> resolve,
            IMenuManager menuManager = null, IPersistenceService persistenceService = null)
        {
            _register = register;
            _resolve = resolve;
            _menuManager = menuManager;
            _persistenceService = persistenceService;
        }

        #region Implementation of IScreenContainer

        public void Register(Type idType, Type viewType, Type logicType)
        {
            CheckParameters(idType, viewType, logicType);

            var popupAttribute = idType.GetCustomAttribute<PopupAttribute>(true);
            _factories[idType] = new ScreenFactory(idType, viewType, logicType, _register, _resolve, popupAttribute);

            if (_menuManager != null)
            {
                var attribute = idType.GetCustomAttributes(typeof(ScreenMenuItemAttribute), true)
                      .OfType<ScreenMenuItemAttribute>()
                      .FirstOrDefault();
                if (attribute != null)
                {
                    var action = attribute.IsPopup
                        ? new Action(() => Popup(idType))
                        : () => Display(idType);
                    _menuManager.Add(attribute.Path, action, attribute.DisplayName, attribute.IconFilePath);
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
            if (_screens.TryGetValue(key, out var host))
                host.SetParameter(parameter);
        }

        public object GetParameter(Type idType, string instanceId)
        {
            var key = new NamedType(idType, instanceId);
            return _screens.TryGetValue(key, out var host) ? host.GetParameter() : null;
        }

        public void Close(Type idType, string instanceId = null)
        {
            var key = new NamedType(idType, instanceId);
            if (_screens.TryGetValue(key, out var host))
                host.Close();
        }

        public void CloseAll(Type idType)
        {
            foreach (var key in _screens.Select(p => p.Key).Where(p => p.Type == idType).ToArray())
                Close(key.Type, key.Name);
        }

        #endregion

        private static void CheckParameters(Type idType, Type viewType, Type logicType)
        {
            if(idType == null)
                throw new ArgumentNullException(nameof(idType));
            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));
            if (logicType == null)
                throw new ArgumentNullException(nameof(logicType));

            if (!idType.InheritsFrom(typeof(IScreen)))
                throw new ArgumentException("The idType which allows you to resolve and display screen has to implement IScreen interface, but was: " + idType.FullName, nameof(idType));
            if (!viewType.InheritsFrom(typeof(TBaseView)))
                throw new ArgumentException("The viewType which allows you to resolve and display screen has to inherit from " + typeof(TBaseView).FullName + ", but was: " + viewType.FullName, nameof(viewType));
            if (!logicType.InheritsFrom(idType))
                throw new ArgumentException("The logicType which will resolve and display screen has to implement " + idType.FullName + " interface, but was: " + logicType.FullName, nameof(logicType));
        }

        private void Show(Type idType, string instanceId, object parameter, bool isPopup)
        {
            var key = new NamedType(idType, instanceId);
            if (_screens.TryGetValue(key, out var host))
            {
                host.BringToFront(isPopup);
                return;
            }

            if (!_factories.TryGetValue(key.Type, out var factory)) return;

            if (!factory.TryCreate(instanceId, parameter, out var logic, out var view, out var e))
            {
                OnException($"Unable to create screen: {key}", e);
                return;
            }

            host = new ScreenHost(key, view, logic, CreateScreen, CreatePopup, factory.PopupAttribute);
            _screens.Add(key, host);
            host.Closed += OnHostClosed;

            if(logic is IInternalScreen internalScreen)
                internalScreen.Setup(host.Close);

            if (!host.Restore(_persistenceService, out e))
                OnException($"Unable to restore screen: {key}", e);

            host.Show(isPopup);
        }

        private void OnHostClosed(ScreenHost host)
        {
            host.Closed -= OnHostClosed;
            _screens.Remove(host.Key);
            if(!host.Persist(_persistenceService, out var e))
                OnException($"Unable to restore screen: {host.Key}", e);
            host.Dispose();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            foreach (var key in _screens.Select(p => p.Key).ToArray())
                Close(key.Type, key.Name);
            _factories.Clear();
        }

        #endregion

        protected abstract IScreenHost CreateScreen(TBaseView view);

        protected abstract IScreenHost CreatePopup(TBaseView view, PopupAttribute popupAttribute);

        protected abstract void OnException(string message, Exception e);

        #region Nested type

        private class ScreenFactory
        {
            private readonly Type _idType;
            private readonly Func<Type, object> _resolve;
            private readonly Func<IScreen, TBaseView> _factory;

            public PopupAttribute PopupAttribute { get; }
            
            public ScreenFactory(Type idType, Type viewType, Type logicType, Action<Type, Type> register, Func<Type, object> resolve, PopupAttribute popupAttribute)
            {
                _idType = idType;
                _resolve = resolve;
                PopupAttribute = popupAttribute;

                _factory = idType != viewType
                    ? viewType.CreateFactory<TBaseView>(idType)
                    : (p => (TBaseView)p);

                register(idType, logicType);
            }

            public bool TryCreate(string instanceId, object parameter, out IScreen logic, out TBaseView view, out Exception ex)
            {
                ex = null;
                try
                {
                    logic = _resolve(_idType) as IScreen;
                    if (logic is IInternalScreen internalScreen)
                    {
                        internalScreen.Setup(instanceId);
                        internalScreen.Parameter = parameter;
                    }

                    view = _factory(logic);
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
            private readonly NamedType _key;
            private readonly TBaseView _view;
            private readonly IScreen _screen;
            private readonly Func<TBaseView, IScreenHost> _createScreen;
            private readonly Func<TBaseView, PopupAttribute, IScreenHost> _createPopup;
            private readonly PopupAttribute _popupAttribute;

            private bool _isInPopup;
            private IScreenHost _host;

            public NamedType Key => _key;

            public ScreenHost(NamedType key,
                TBaseView view,
                IScreen screen,
                Func<TBaseView, IScreenHost> createScreen,
                Func<TBaseView, PopupAttribute, IScreenHost> createPopup,
                PopupAttribute popupAttribute)
            {
                _key = key;
                _view = view;
                _screen = screen;
                _createScreen = createScreen;
                _createPopup = createPopup;
                _popupAttribute = popupAttribute;
                _screen.PropertyChanged += OnPropertyChanged;
            }

            public event Action<ScreenHost> Closed;

            public void Close()
            {
                _host.Close();
            }

            public void Show(bool isPopup)
            {
                _host = isPopup ? _createPopup(_view, _popupAttribute) : _createScreen(_view);

                if (_screen != null)
                    _host.Title = _screen.Title;

                _host.ScreenClosed += OnScreenClosed;
                _isInPopup = isPopup;
            }

            public void BringToFront(bool isPopup)
            {
                if (isPopup == _isInPopup)
                    _host.BringToFront();
                else
                {
                    _host.ScreenClosed -= OnScreenClosed;
                    _host.Close();
                    Show(isPopup);
                }
            }

            private void OnScreenClosed(IScreenHost screenHost) 
                => Closed?.Invoke(this);

            public void SetParameter(object parameter)
            {
                if (_screen is IInternalScreen cast)
                    cast.Parameter = parameter;
            }

            public object GetParameter()
            {
                return _screen is IInternalScreen cast ? cast.Parameter : null;
            }

            #region Persistency

            public bool Restore(IPersistenceService svc, out Exception ex)
            {
                try
                {
                    ex = null;
                    if (svc == null) return true;

                    if (_view is IPersistable persistable)
                        svc.Restore(GetViewName(), persistable);
                    
                    if (ReferenceEquals(_screen, _view)) return true; // In case of there is no screen type defined

                    // ReSharper disable SuspiciousTypeConversion.Global
                    persistable = _screen as IPersistable;
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

                    if (_view is IPersistable persistable)
                        svc.Persist(GetViewName(), persistable);
                    if (ReferenceEquals(_screen, _view)) return true; // In case of there is no screen type defined

                    // ReSharper disable SuspiciousTypeConversion.Global
                    persistable = _screen as IPersistable;
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

            private string GetViewName() => $"{_key.Type}_{_key.Name}_View";

            private string GetLogicName() => $"{_key.Type}_{_key.Name}_Logic";

            #endregion

            #region Manage screen events

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if(e.PropertyName == "Title")
                    _host.Title = _screen.Title;
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                _screen.PropertyChanged -= OnPropertyChanged;
                _host.ScreenClosed -= OnScreenClosed;
                _host = null;

                if (_screen is IDisposable disposable)
                    disposable.Dispose();
            }

            #endregion

            public override string ToString() => $"View: {_view}, Logic: {_screen}";
        }

        #endregion
    }
}