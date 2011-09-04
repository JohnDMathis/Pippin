using Microsoft.Practices.Prism.Events;
using Pippin.UI.Regions;
using Pippin.UI.Screens;
using System;

namespace Pippin.UI.Events
{

    public class ScreenEvent : CompositePresentationEvent<ScreenEventArgs>
    {
        
    }

    public static class EventAggregator_Extensions_For_Screen_Events
    {
        public static void ScreenActivate(this IEventAggregator ea, string screenName)
        {
            var sea = new ScreenEventArgs { Event = ScreenEventType.Activate, ScreenName = screenName };
            ea.GetEvent<ScreenEvent>().Publish(sea);
        }

        public static void ScreenActivate(this IEventAggregator ea, string screenName, object subject)
        {
            var sea = new ScreenEventArgs { Event = ScreenEventType.Activate, ScreenName = screenName, ScreenSubject = subject };
            ea.GetEvent<ScreenEvent>().Publish(sea);
        }

        public static void ScreenActivate(this IEventAggregator ea, string screenName, object subject, RegionName region)
        {
            var sea = new ScreenEventArgs { Event = ScreenEventType.Activate, ScreenName = screenName, ScreenSubject = subject, RegionName=region };
            ea.GetEvent<ScreenEvent>().Publish(sea);
        }

        public static void ScreenDeactivate(this IEventAggregator ea, string screenName)
        {
            var sea = new ScreenEventArgs { Event = ScreenEventType.Deactivate, ScreenName = screenName };
            ea.GetEvent<ScreenEvent>().Publish(sea);
        }

        public static void ScreenEvent(this IEventAggregator ea, ScreenEventArgs sea)
        {
            ea.GetEvent<ScreenEvent>().Publish(sea);
        }

        public static SubscriptionToken SubscribeToScreenEvent(this IEventAggregator ea, Action<ScreenEventArgs> action)
        {
            return ea.GetEvent<ScreenEvent>().Subscribe(action);
        }
    }



}