using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Pippin.UI.Events;
using Pippin.UI.Screens;
using Pippin.UI.ViewModel;
using Pippin.UI.Events;

namespace Pippin
{
    public abstract class ModuleBase : IModule
    {
        #region Properties

        protected IUnityContainer Container { get; set; }
        protected IScreenFactoryRegistry ScreenFactoryRegistry { get; set; }
        protected IEventAggregator EventManager{get;set;}

        #endregion

        #region Constructors

        public ModuleBase(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IEventAggregator eventMgr)
        {
            Container = container;
            ScreenFactoryRegistry = screenFactoryRegistry;
            EventManager=eventMgr;
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
            EventManager.SendModuleLoaded(name);
        }

        protected abstract void RegisterScreenFactories();

        protected abstract void RegisterViewsAndServices();

        #endregion
    }
}
