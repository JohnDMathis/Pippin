using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using Odin.UI.Infrastructure.Events;
using Odin.UI.Infrastructure.ScreenFramework;
using Odin.UI.Infrastructure.ViewModel;

namespace Odin.UI.Infrastructure.Module
{
    public abstract class ModuleBase : EventAggregatorClient, IModule
    {
        #region Properties

        protected IUnityContainer Container { get; set; }
        protected IScreenFactoryRegistry ScreenFactoryRegistry { get; set; }

        #endregion

        #region Constructors

        public ModuleBase(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry)
        {
            Container = container;
            ScreenFactoryRegistry = screenFactoryRegistry;
        }

        #endregion

        #region Implementation of IModule

        public virtual void Initialize()
        {
            RegisterViewsAndServices();
            RegisterScreenFactories();
            string name = GetType().FullName;
            var index = name.LastIndexOf('.');
            name = name.Remove(index);
            Container.Resolve<IEventAggregator>().GetEvent<ModuleLoadedEvent>().Publish(name);
        }

        protected abstract void RegisterScreenFactories();

        protected abstract void RegisterViewsAndServices();

        #endregion
    }
}
