using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Pippin.UI.Conductors;
using Pippin.UI.Events;
using Pippin.UI;
using Pippin.UI.Regions;
using Pippin.UI.Screens;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;

namespace IntelShell
{

    /// <summary>
    /// This class handles major application-wide events in the app lifetime.
    /// Specifically: Startup, Login, Logout
    /// </summary>
    public class ApplicationController : IApplicationController
    {
        private IUnityContainer _container;
       // private LookupManager _lookupManager;
        private IModuleManager _moduleManager;
        private List<string> _modulesToLoad;
        private List<string> _modulesLoaded;
        private IEventAggregator _eventMgr;

        public ApplicationController(IUnityContainer container, IModuleManager moduleManager, IEventAggregator eventMgr)
        {
            _container = container;
            _moduleManager = moduleManager;
            _eventMgr = eventMgr;
            _modulesToLoad = new List<string>();
            _modulesLoaded = new List<string>();
        }

        public void Startup()
        {
            RegisterServices();
            ConfigureScreenFramework();
            // subscribe to events
            //Subscribe<LogonEvent>(HandleLogonEvent);
            //Subscribe<ModuleLoadedEvent>(HandleModuleLoaded);
            _eventMgr.SubscribeTo_ModuleLoadedEvent(HandleModuleLoaded);
            _eventMgr.Screen_Activate("ServiceBoard.Calendar", null, RegionName.Body);
        }

        public void ConfigureScreenFramework()
        {            
            _container.RegisterType<IScreenFactoryRegistry, ScreenFactoryRegistry>(new ContainerControlledLifetimeManager());
            _container.RegisterType<MainRegionConductor>(new ContainerControlledLifetimeManager());
            _container.RegisterType<HeaderConductor>(new ContainerControlledLifetimeManager());
            _container.RegisterType<FooterConductor>(new ContainerControlledLifetimeManager());
            _container.RegisterType<OverlayConductor>(new ContainerControlledLifetimeManager());

            _container.Resolve<MainRegionConductor>();
            _container.Resolve<HeaderConductor>();
            _container.Resolve<FooterConductor>();
            _container.Resolve<OverlayConductor>();
        }

        public void RegisterServices()
        {
           // _container.RegisterType<LookupManager>();
            //_container.RegisterType<ServiceBoardService>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<AuthenticationContext>(new ContainerControlledLifetimeManager())
            //    .Configure<InjectedMembers>()
            //    .ConfigureInjectionFor<AuthenticationContext>(new InjectionConstructor());
            //_container.RegisterType<CoreContext>(new ContainerControlledLifetimeManager())
            //    .Configure<InjectedMembers>()
            //    .ConfigureInjectionFor<CoreContext>(new InjectionConstructor());
            //_container.RegisterType<LookupContext>(new ContainerControlledLifetimeManager())
            //    .Configure<InjectedMembers>()
            //    .ConfigureInjectionFor<LookupContext>(new InjectionConstructor());
            //_container.RegisterType<EmploymentContext>(new ContainerControlledLifetimeManager())
            //    .Configure<InjectedMembers>()
            //    .ConfigureInjectionFor<EmploymentContext>(new InjectionConstructor());

            //// create external references
            //var coreContext = _container.Resolve<CoreContext>();
            //var lookupContext = _container.Resolve<LookupContext>();
            //var employmentContext = _container.Resolve<EmploymentContext>();

            //coreContext.AddReference(typeof(State), lookupContext);
            //coreContext.AddReference(typeof(TelephoneType), lookupContext);
            //coreContext.AddReference(typeof(Hierarchy), lookupContext);

            //employmentContext.AddReference(typeof(Veteran), coreContext);
            //employmentContext.AddReference(typeof(User), coreContext);
            //employmentContext.AddReference(typeof(Address), coreContext);
            //employmentContext.AddReference(typeof(FormTemplate), coreContext);
            //employmentContext.AddReference(typeof(PlanStatusType), lookupContext);
            //employmentContext.AddReference(typeof(PlanOutcome), lookupContext);
            //employmentContext.AddReference(typeof(BarrierType), lookupContext);
            //employmentContext.AddReference(typeof(PlanEventType), lookupContext);
            //employmentContext.AddReference(typeof(PlanNoteType), lookupContext);
            //employmentContext.AddReference(typeof(ProgramType), lookupContext);
            //employmentContext.AddReference(typeof(Hierarchy), lookupContext);
            //employmentContext.AddReference(typeof(DisabilityRating), lookupContext);

            //// register providers
            //_container.RegisterType<LookupManager>();
            //_lookupManager = _container.Resolve<LookupManager>();
        }

        public void HandleLogonEvent(object obj)
        {
            // sent by the CurrentUser object; handle any tasks that need to be performed after the user has been logged on.
            Debug.WriteLine("HandleLogonEvent()");
            //_lookupManager.Load();
            LoadUserModules();
            if (_modulesToLoad.Count == 0)
                // nothing needs to be loaded; just start the UI.  (this is generally the case if user logs out, then back in
                StartUser();
        }

        public void HandleModuleLoaded(object obj)
        {
            string moduleName = obj.ToString();
            int i = moduleName.LastIndexOf('.');
            moduleName = moduleName.Remove(0, i + 1);
            // if this module is not one of the ones that are explicitly loaded, just ignore it; we don't care about
            // the ones that are loaded immediately
            if (!_modulesToLoad.Contains(moduleName)) return;

            _modulesToLoad.Remove(moduleName);
            _modulesLoaded.Add(moduleName);
            Debug.WriteLine("Loaded: " + moduleName + " (" + _modulesToLoad.Count + " remaining)");
            if (_modulesToLoad.Count == 0)
            {
                StartUser();
            }
        }

        private void LoadUserModules()
        {
            // load only those modules that the currentUser is allowed to access

            //if (CurrentUser.Can(ActionItem.ViewVeteran))
            //    LoadModule("Veterans");

            //if (CurrentUser.Can(ActionItem.AccessEDP))
            //    LoadModule("Employment");

            //if (CurrentUser.Can(ActionItem.ManageUsers)
            //    || CurrentUser.Can(ActionItem.EditLookups)
            //    || CurrentUser.Can(ActionItem.ResetPassword))
            //    LoadModule("Admin");

            //if (CurrentUser.Can(ActionItem.ViewReports))
            //    LoadModule("Reports");
        }

        private void LoadModule(string moduleName)
        {
            // has the module already been loaded? if not, load it
            if (!_modulesLoaded.Contains(moduleName))
            {
                _modulesToLoad.Add(moduleName);
                _moduleManager.LoadModule(moduleName);
            }
        }

        private void StartUser()
        {
            //if (CurrentUser.Entity.PasswordChangeRequired)
            //    SendScreenEvent(new ScreenEventArgs { ScreenKey = ScreenKeyType.ChangePswd });
            //else
            //    CurrentUser.GoHome();
        }

        //public void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        //{
        //    // report the exception to user
        //    EventManager.GetEvent<UnhandledExceptionEvent>().Publish(e);

        //    // send to server for logging and forwarding via email

        //}
    }
}