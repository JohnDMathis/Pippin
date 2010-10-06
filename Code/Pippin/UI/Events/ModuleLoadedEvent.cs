using System;
using Microsoft.Practices.Prism.Events;

namespace Pippin.UI.Events
{
    public class ModuleLoadedEvent : CompositePresentationEvent<object>
    {
    }

    public static class EventAggregator_Extensions_ModuleLoadedEvents
    {
        public static void SendModuleLoaded(this IEventAggregator ea, string name)
        {
            ea.GetEvent<ModuleLoadedEvent>().Publish(name);
        }

        public static SubscriptionToken SubscribeTo_ModuleLoadedEvent(this IEventAggregator ea, Action<object> action)
        {
            return ea.GetEvent<ModuleLoadedEvent>().Subscribe(action);
        }

    }

}