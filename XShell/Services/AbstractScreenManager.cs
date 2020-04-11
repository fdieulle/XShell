using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using XShell.Core;

namespace XShell.Services
{
    public abstract class AbstractScreenManager<TBaseView, TScreen, TPopup> : IScreenManager, IScreenContainer
        where TBaseView : class
        where TScreen : IScreenHost
        where TPopup : IPopupScreenHost
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

        public void Popup(Type idType, string instanceId = null, object parameter = null, PopupAttribute popupAttribute = null)
        {
            Show(idType, instanceId, parameter, true, popupAttribute);
        }

        //public TResult Dialog<TResult>(Type idType, string instanceId = null, object parameter = null, PopupAttribute popupAttribute = null)
        //{
        //    return Show<TResult>(idType, instanceId, parameter, true, true, popupAttribute);
        //}

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

        public void CloseAll(Type idType = null)
        {
            foreach (var key in _screens.Select(p => p.Key).Where(p => p == null || p.Type == idType).ToArray())
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

        private ScreenHost CreateScreenHost(Type idType, string instanceId, Func<IInternalScreen, object> getParameter, bool isPopup, PopupAttribute popupAttribute = null)
        {
            var key = new NamedType(idType, instanceId);
            if (_screens.TryGetValue(key, out var host))
            {
                host.BringToFront(isPopup);
                return null;
            }

            if (!_factories.TryGetValue(key.Type, out var factory)) return null;

            if (!factory.TryCreate(instanceId, getParameter, out var logic, out var view, out var e))
            {
                OnException($"Unable to create screen: {key}", e);
                return null;
            }

            popupAttribute = popupAttribute ?? factory.PopupAttribute;
            host = new ScreenHost(key, view, logic, CreateScreen, ShowScreen, CreatePopup, ShowPopup, popupAttribute);
            _screens.Add(key, host);
            host.Closed += OnHostClosed;

            if (logic is IInternalScreen internalScreen)
                internalScreen.Setup(host.Close);

            if (!host.Restore(_persistenceService, out e))
                OnException($"Unable to restore screen: {key}", e);

            return host;
        }

        private void Show(Type idType, string instanceId, object parameter, bool isPopup, PopupAttribute popupAttribute = null)
        {
            var host = CreateScreenHost(idType, instanceId, s => parameter, isPopup, popupAttribute);

            host?.Show(isPopup);
        }

        private void OnHostClosed(ScreenHost host)
        {
            host.Closed -= OnHostClosed;
            _screens.Remove(host.Key);
            if(!host.Persist(_persistenceService, out var e))
                OnException($"Unable to restore screen: {host.Key}", e);
            host.Dispose();
        }

        #region Manage workspaces

        public void SaveWorkspace(Stream stream)
        {
            var screens = _screens.Values.ToList();
            var memory = new MemoryStream();
            stream.Write(screens.Count);
            foreach (var screen in screens)
            {
                var settings = screen.GetWorkspaceSettings();

                ScreenSettings.Serialize(memory, settings);
                memory.Commit(stream);

                screen.SerializeParameter(memory);
                memory.Commit(stream);
            }
        }

        public Dictionary<string, TScreen> LoadWorkspace(Stream stream)
        {
            var result = new Dictionary<string, TScreen>();
            var memory = new MemoryStream();
            var count = memory.ReadInt32(stream);

            for (var i = 0; i < count; i++)
            {
                memory.Fetch(stream);
                var settings = ScreenSettings.Deserialize(memory);

                memory.Fetch(stream);
                var screen = CreateScreenHost(
                    Type.GetType(settings.Type),
                    settings.Name,
                    s => s.DeserializeParameter(memory),
                    settings.IsPopup,
                    settings.ToPopupAttribute());

                if (settings.IsPopup)
                    screen.Show(true);
                else result[settings.PersistenceId] = screen.CreateScreen();
            }

            return result;
        }
        
        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            foreach (var key in _screens.Select(p => p.Key).ToArray())
                Close(key.Type, key.Name);
            _factories.Clear();
        }

        #endregion

        protected abstract TScreen CreateScreen(TBaseView view);

        protected abstract void ShowScreen(TScreen screen);

        protected abstract TPopup CreatePopup(TBaseView view, PopupAttribute popupAttribute);

        protected abstract void ShowPopup(TPopup popup);

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

            public bool TryCreate(string instanceId, Func<IInternalScreen, object> getParameter, out IScreen logic, out TBaseView view, out Exception ex)
            {
                ex = null;
                try
                {
                    logic = _resolve(_idType) as IScreen;
                    if (logic is IInternalScreen internalScreen)
                    {
                        internalScreen.Setup(instanceId);
                        internalScreen.Parameter = getParameter(internalScreen);
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
            private readonly TBaseView _view;
            private readonly IScreen _screen;
            private readonly Func<TBaseView, TScreen> _createScreen;
            private readonly Action<TScreen> _showScreen;
            private readonly Func<TBaseView, PopupAttribute, TPopup> _createPopup;
            private readonly Action<TPopup> _showPopup;
            private readonly PopupAttribute _popupAttribute;

            private bool _isPopup;

            public NamedType Key { get; }

            public IScreenHost Host { get; private set; }

            public ScreenHost(NamedType key,
                TBaseView view,
                IScreen screen,
                Func<TBaseView, TScreen> createScreen,
                Action<TScreen> showScreen,
                Func<TBaseView, PopupAttribute, TPopup> createPopup,
                Action<TPopup> showPopup,
                PopupAttribute popupAttribute)
            {
                Key = key;
                _view = view;
                _screen = screen;
                _createScreen = createScreen;
                _showScreen = showScreen;
                _createPopup = createPopup;
                _showPopup = showPopup;
                _popupAttribute = popupAttribute;
                _screen.PropertyChanged += OnPropertyChanged;
            }

            public event Action<ScreenHost> Closed;

            public void Close() => Host.Close();

            public TScreen CreateScreen()
            {
                var host = _createScreen(_view);
                if (_screen != null)
                    host.Title = _screen.Title;
                host.ScreenClosed += OnScreenClosed;
                Host = host;
                _isPopup = false;
                return host;
            }

            public void Show(bool isPopup)
            {
                if (isPopup)
                {
                    var host = _createPopup(_view, _popupAttribute);
                    if (_screen != null)
                        host.Title = _screen.Title;
                    host.ScreenClosed += OnScreenClosed;
                    Host = host;
                    _isPopup = true;
                    _showPopup(host);
                }
                else
                {
                    var host = CreateScreen();
                    _showScreen(host);
                }
            }

            public void BringToFront(bool isPopup)
            {
                if (isPopup == _isPopup)
                    Host.BringToFront();
                else
                {
                    Host.ScreenClosed -= OnScreenClosed;
                    Host.Close();
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

            public object GetParameter() => _screen is IInternalScreen cast ? cast.Parameter : null;

            #region Persistency

            public ScreenSettings GetWorkspaceSettings()
            {
                var settings = new ScreenSettings
                {
                    PersistenceId = Host.GetPersistenceId(),
                    Type = Key.Type.AssemblyQualifiedName != null 
                        ? string.Join(",", Key.Type.AssemblyQualifiedName.Split(',').Take(2))
                        : Key.Type.FullName,
                    Name = Key.Name,
                    IsPopup = _isPopup
                };

                if (_popupAttribute != null)
                {
                    settings.TopMost = _popupAttribute.TopMost;
                    settings.ResizeMode = _popupAttribute.ResizeMode;
                    settings.Icon = _popupAttribute.Icon;
                }

                if (Host is IPopupScreenHost popup)
                    settings.PositionAndSize = popup.GetPositionAndSize() ?? new RectangleSettings { Height = 300, Width = 300 };

                return settings;
            }

            public void SerializeParameter(Stream stream)
            {
                if (_screen is IInternalScreen screen)
                    screen.SerializeParameter(stream);
            }

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

            private string GetViewName() => $"{Key.Type}_{Key.Name}_View";

            private string GetLogicName() => $"{Key.Type}_{Key.Name}_Logic";

            #endregion

            #region Manage screen events

            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if(e.PropertyName == nameof(IScreen.Title))
                    Host.Title = _screen.Title;
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                _screen.PropertyChanged -= OnPropertyChanged;
                Host.ScreenClosed -= OnScreenClosed;
                Host = null;

                if (_screen is IDisposable disposable)
                    disposable.Dispose();
            }

            #endregion

            public override string ToString() => $"View: {_view}, Logic: {_screen}";
        }

        #endregion
    }

    public class ScreenSettings
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(ScreenSettings));

        public static void Serialize(Stream stream, ScreenSettings settings) 
            => serializer.Serialize(stream, settings);

        public static ScreenSettings Deserialize(Stream stream) 
            => (ScreenSettings)serializer.Deserialize(stream);

        public string PersistenceId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsPopup { get; set; }
        public RectangleSettings PositionAndSize { get; set; } = new RectangleSettings();
        public string Icon { get; set; }
        public bool TopMost { get; set; }
        public ResizeMode ResizeMode { get; set; }

        public PopupAttribute ToPopupAttribute() 
            => new PopupAttribute
            {
                StartupLocation = StartupLocation.Manual,
                Width = PositionAndSize.Width,
                Height = PositionAndSize.Height,
                Top = PositionAndSize.Top,
                Left = PositionAndSize.Left,
                Icon = Icon,
                TopMost = TopMost,
                ResizeMode = ResizeMode
            };
    }

    public class RectangleSettings
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}