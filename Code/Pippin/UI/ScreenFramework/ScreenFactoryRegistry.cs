using System;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace Odin.UI.Infrastructure.ScreenFramework
{
    public class ScreenFactoryRegistry : IScreenFactoryRegistry
    {
        protected IUnityContainer Container { get; set; }
        protected IDictionary<ScreenKeyType, Type> ScreenFactoryCollection { get; set; }

        public ScreenFactoryRegistry(IUnityContainer container)
        {
            this.Container = container;
            this.ScreenFactoryCollection = new Dictionary<ScreenKeyType, Type>();
        }

        public IScreenFactory Get(ScreenKeyType screenType)
        {
            IScreenFactory screenFactory = null;
            if (this.HasFactory(screenType))
            {
                Type registeredScreenFactory = ScreenFactoryCollection[screenType];
                screenFactory = (IScreenFactory)Container.Resolve(registeredScreenFactory);
            }
            return screenFactory;
        }

        public void Register(ScreenKeyType screenType, Type registeredScreenFactoryType)
        {
            if (!HasFactory(screenType))
                this.ScreenFactoryCollection.Add(screenType, registeredScreenFactoryType);
        }


        public bool HasFactory(ScreenKeyType screenType)
        {
            return (this.ScreenFactoryCollection.ContainsKey(screenType));
        }

    }
}
