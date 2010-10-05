using System;
using Microsoft.Practices.Prism.Events;

namespace Pippin.UI.Events
{

    public class Thing
    {
        public int Id{get;set;}
    }

    public class ThingOpenedEvent:CompositePresentationEvent<Thing>{}

    public static class EventAggregatorExtensions
    {

        public static void ThingOpened(this IEventAggregator eventMgr, Thing thing)
        {
            eventMgr.GetEvent<ThingOpenedEvent>().Publish(thing);
        }

        public static void Unsubscribe<T>(this IEventAggregator eventMgr, SubscriptionToken token) where T:EventBase
        {
            eventMgr.GetEvent<T>().Unsubscribe(token);
        }
    }

}