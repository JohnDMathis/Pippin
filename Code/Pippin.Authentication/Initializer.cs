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
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Pippin;
using Pippin.UI.Modules;
using Pippin.UI.Events;
using Pippin.UI.Regions;
using Pippin.UI.Screens;
using MI.Authentication.Screens;
using MI.Authentication.ViewModel;
using MI.Services;

namespace MI.Authentication
{
    public class Initializer : ModuleBase
    {
        public Initializer(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IEventAggregator ea)
            : base(container, screenFactoryRegistry, ea) { }

        public override void Initialize()
        {
            base.Initialize();
            Container.Resolve<CurrentUser>();
        }

        protected override void RegisterScreenFactories()
        {
            ScreenFactoryRegistry.Register("EntryScreen", typeof(EntryScreenFactory));
        }

        protected override void RegisterViewsAndServices()
        {
            RegisterIfMissing<EntryViewModel>();
            RegisterIfMissing<AuthenticationContext>(true, new InjectionConstructor());
        }

    }
}
