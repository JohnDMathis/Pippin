using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Modularity;
using Modularity = Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Logging;
using Pippin;
using Pippin.UI;
using Pippin.UI.Modules;
using Pippin.UI.Events;

namespace Pippin
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected UserControl ShellView;
        protected string CatalogPath;

        protected virtual void Initialize()
        {
        }

        public Bootstrapper()
        {
            Initialize();
        }

        public Bootstrapper(UserControl shellView, string catalogPath)
        {
            ShellView = (UserControl)shellView;
            CatalogPath = catalogPath;
        }

        private readonly CallbackLogger callbackLogger = new CallbackLogger();


        protected override ILoggerFacade CreateLogger()
        {
            this.callbackLogger.Callback = (s, c, p) => LogCallback(s, c, p);
            return this.callbackLogger;
        }

        public void LogCallback(string s, Category c, Priority p)
        {
            Debug.WriteLine(s);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            //var catalogUri = new Uri("/ClearImage.App;component/ModuleCatalog.xaml", UriKind.Relative);
            var catalogUri = new Uri(CatalogPath, UriKind.Relative);
            return Modularity.ModuleCatalog.CreateFromXaml(catalogUri);
        }

        protected override void ConfigureContainer()
        {
            //------------------------------------
            //base.ConfigureContainer();
            base.Logger.Log("AddingUnityBootstrapperExtensionToContainer", Category.Debug, Priority.Low);
            this.Container.AddNewExtension<UnityBootstrapperExtension>();

            Container.RegisterInstance<ILoggerFacade>(Logger);

            this.Container.RegisterInstance(this.ModuleCatalog);
            RegisterTypeIfMissing(typeof(IServiceLocator), typeof(UnityServiceLocatorAdapter), true);
            RegisterTypeIfMissing(typeof(IModuleInitializer), typeof(ModuleInitializer), true);
            RegisterTypeIfMissing(typeof(IModuleManager), typeof(ModuleManager), true);
            RegisterTypeIfMissing(typeof(RegionAdapterMappings), typeof(RegionAdapterMappings), true);
            RegisterTypeIfMissing(typeof(IRegionManager), typeof(RegionManager), true);
            RegisterTypeIfMissing(typeof(IEventAggregator), typeof(EventAggregator), true);
            RegisterTypeIfMissing(typeof(IRegionViewRegistry), typeof(RegionViewRegistry), true);
            RegisterTypeIfMissing(typeof(IRegionBehaviorFactory), typeof(RegionBehaviorFactory), true);

            //RegisterTypeIfMissing(typeof(IApplicationController), typeof(ApplicationController), true);
            RegisterTypeIfMissing(typeof(Pippin.Configurator), typeof(Pippin.Configurator), true);

            RegisterApplicationController();

            //RegisterTypeIfMissing(typeof(IRegionNavigationJournalEntry), typeof(RegionNavigationJournalEntry), false);
            //RegisterTypeIfMissing(typeof(IRegionNavigationJournal), typeof(RegionNavigationJournal), false);
            //RegisterTypeIfMissing(typeof(IRegionNavigationService), typeof(RegionNavigationService), false);
            // RegisterTypeIfMissing(typeof(IRegionNavigationContentLoader), typeof(RegionNavigationContentLoader), true);          




            //------------------------------------
            // don't need this any longer because we can use servicelocator
            //Common.Container.Instance = Container;
        }

        protected override DependencyObject CreateShell()
        {
  //          var shell = Container.Resolve<IShellView>() as UserControl;
            return ShellView;
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.RootVisual = (UIElement)this.Shell;
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            base.Logger.Log("Starting Application-specific configuration.", Category.Debug, Priority.Low);

            // Configure Pippin libraries (primarily screen framework / conductors)
            Container.Resolve<Pippin.Configurator>().Configure();

            // Use ApplicationController to handle app-specific startup tasks
            //Container.Resolve<IApplicationController>().Startup();
            StartApplication();
        }

        protected virtual void RegisterApplicationController()
        {
        }

        protected virtual void StartApplication()
        {
        }
    }
}
