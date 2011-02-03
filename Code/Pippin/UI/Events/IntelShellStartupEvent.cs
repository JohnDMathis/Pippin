using Microsoft.Practices.Prism.Events;
using Pippin.UI.Regions;
using Pippin.UI.Screens;
using System;

namespace Pippin.UI.Events
{
    public class IntelShellStartupEvent : CompositePresentationEvent<object>
    {
    }

    public static class EventAggregator_Extensions_For_StartupEvent
    {
        public static void IntelShellStartup(this IEventAggregator ea)
        {
            ea.GetEvent<IntelShellStartupEvent>().Publish(null);
        }

        public static SubscriptionToken SubscribeToIntelShellStartupEvent(this IEventAggregator ea, Action<object> action)
        {
            return ea.GetEvent<IntelShellStartupEvent>().Subscribe(action);
        }

    }

}
