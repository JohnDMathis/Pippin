using System;

using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;
using Pippin.UI.Events;
using Pippin.UI.Screens;
using Pippin.UI.ViewModel;

namespace Pippin
{
    public abstract class ModuleBase : IModule
    {
        #region Properties

        protected IUnityContainer Container { get; set; }
        protected IScreenFactoryRegistry ScreenFactoryRegistry { get; set; }
        protected IEventAggregator EventManager { get; set; }

        #endregion

        #region Constructors

        public ModuleBase(IUnityContainer container, IScreenFactoryRegistry screenFactoryRegistry, IEventAggregator eventMgr)
        {
            Container = container;
            ScreenFactoryRegistry = screenFactoryRegistry;
            EventManager = eventMgr;
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

        #region helpers
        //
        // Summary:
        //     Registers a type in the container only if that type was not already registered.
        //     Borrowed from Prism.
        // Parameters:
        //   fromType:
        //     The interface type to register.
        //
        //   toType:
        //     The type implementing the interface.
        //
        //   registerAsSingleton:
        //     Registers the type as a singleton.
        protected bool RegisterIfMissing<Tfrom, Tto>(bool registerAsSingleton) where Tto : Tfrom
        {
            if (Container.IsRegistered<Tfrom>()) return false;
            if (registerAsSingleton)
            {
                Container.RegisterType<Tfrom, Tto>(new ContainerControlledLifetimeManager());
            }
            else
            {
                Container.RegisterType<Tfrom, Tto>();
            }
            return true;
        }


        protected bool RegisterIfMissing<T>(bool registerAsSingleton)
        {
            if (Container.IsRegistered<T>()) return false;
            if (registerAsSingleton)
            {
                Container.RegisterType<T>(new ContainerControlledLifetimeManager());
            }
            else
            {
                Container.RegisterType<T>();
            }
            return true;
        }

        protected bool RegisterIfMissing<T>(bool registerAsSingleton, InjectionConstructor constructor)
        {
            if (Container.IsRegistered<T>()) return false;
            if (registerAsSingleton)
            {
                Container.RegisterType<T>(new ContainerControlledLifetimeManager(), constructor);
            }
            else
            {
                Container.RegisterType<T>(constructor);
            }
            return true;
        }


        protected bool RegisterIfMissing<T>()
        {
            return RegisterIfMissing<T>(false);
        }



        #endregion

    }
}

