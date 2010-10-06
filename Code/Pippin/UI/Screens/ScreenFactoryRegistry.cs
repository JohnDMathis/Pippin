using System;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace Pippin.UI.Screens
{
    public class ScreenFactoryRegistry : IScreenFactoryRegistry
    {
        protected IUnityContainer Container { get; set; }
        protected IDictionary<string, Type> ScreenFactoryCollection { get; set; }

        public ScreenFactoryRegistry(IUnityContainer container)
        {
            this.Container = container;
            this.ScreenFactoryCollection = new Dictionary<string, Type>();
        }

        public IScreenFactory Get(string screenName)
        {
            IScreenFactory screenFactory = null;
            if (this.HasFactory(screenName))
            {
                Type registeredScreenFactory = ScreenFactoryCollection[screenName];
                screenFactory = (IScreenFactory)Container.Resolve(registeredScreenFactory);
            }
            return screenFactory;
        }

        public void Register(string screenName, Type registeredScreenFactoryType)
        {
            if (!HasFactory(screenName))
                this.ScreenFactoryCollection.Add(screenName, registeredScreenFactoryType);
        }


        public bool HasFactory(string screenName)
        {
            return (this.ScreenFactoryCollection.ContainsKey(screenName));
        }

    }
}
