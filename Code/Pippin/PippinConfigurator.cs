
using Microsoft.Practices.Unity;
using Pippin.UI.Conductors;
using Pippin.UI.Screens;
using Pippin.UI.Events;
using Pippin.UI.VisibilityServices;

namespace Pippin
{
    public class Configurator
    {
        IUnityContainer _container { get; set; }

        public Configurator(IUnityContainer container)
        {
            _container = container;
        }

        public void Configure()
        {
            _container.RegisterType<IScreenFactoryRegistry, ScreenFactoryRegistry>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IVisibilityService, DefaultVisibilityService>();
            _container.RegisterType<MainRegionConductor>(new ContainerControlledLifetimeManager());
            _container.RegisterType<HeaderConductor>(new ContainerControlledLifetimeManager());
            _container.RegisterType<FooterConductor>(new ContainerControlledLifetimeManager());
            _container.RegisterType<OverlayConductor>(new ContainerControlledLifetimeManager());

            _container.Resolve<MainRegionConductor>();
            _container.Resolve<HeaderConductor>();
            _container.Resolve<FooterConductor>();
            _container.Resolve<OverlayConductor>();

        }
    }
}
