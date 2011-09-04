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

        public IScreenFactory Get(string screenId)
        {
            IScreenFactory screenFactory = null;
            if (this.HasFactory(screenId))
            {
                Type registeredScreenFactory = ScreenFactoryCollection[screenId];
                screenFactory = (IScreenFactory)Container.Resolve(registeredScreenFactory);
            }
            return screenFactory;
        }

        public void Register(string screenId, Type registeredScreenFactoryType)
        {
            if (!HasFactory(screenId))
                this.ScreenFactoryCollection.Add(screenId, registeredScreenFactoryType);
        }


        public bool HasFactory(string screenId)
        {
            return (this.ScreenFactoryCollection.ContainsKey(screenId));
        }

    }
}
