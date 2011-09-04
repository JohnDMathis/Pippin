using Microsoft.Practices.Prism.Events;
using System;

namespace Pippin.UI.Events
{
    public class SetOverlayStateChangedEvent : CompositePresentationEvent<bool>
    {
    }

    public static class EventAggregator_Extensions_for_OverlayEvents
    {
        public static void SendOverlayStateChanged(this IEventAggregator ea, bool newState)
        {
            ea.GetEvent<SetOverlayStateChangedEvent>().Publish(newState);
        }

        public static SubscriptionToken SubscribeToOverlayStateChangedEvent(this IEventAggregator ea, Action<bool> action)
        {
            return ea.GetEvent<SetOverlayStateChangedEvent>().Subscribe(action);
        }

    }
}