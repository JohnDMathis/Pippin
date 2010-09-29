using System;
using Microsoft.Practices.Prism.Events;

namespace Pippin.UI.ViewModel
{
    public class EventAggregatorClient
    {
        protected IEventAggregator EventManager { get; set; }

        protected void Send<T, P>(P payload) where T : CompositePresentationEvent<P>
        {
            EventManager.GetEvent<T>().Publish(payload);
        }

        protected void Send<T>(object o) where T : CompositePresentationEvent<object>
        {
            EventManager.GetEvent<T>().Publish(o);
        }

        protected SubscriptionToken Subscribe<T, P>(Action<P> action) where T : CompositePresentationEvent<P>
        {
            var token = EventManager.GetEvent<T>().Subscribe(action);
            return token;
        }

        protected SubscriptionToken Subscribe<T>(Action<object> action) where T : CompositePresentationEvent<object>
        {
            var token = EventManager.GetEvent<T>().Subscribe(action);
            return token;
        }

        protected void Unsubscribe<T,P>(Action<P> action) where T : CompositePresentationEvent<P>
        {
            EventManager.GetEvent<T>().Unsubscribe(action);
        }

        protected void Unsubscribe<T,P>(SubscriptionToken token) where T : CompositePresentationEvent<P>
        {
            EventManager.GetEvent<T>().Unsubscribe(token);
        }

        protected void Unsubscribe<T>(Action<object> action) where T : CompositePresentationEvent<object>
        {
            EventManager.GetEvent<T>().Unsubscribe(action);
        }

        protected void Unsubscribe<T>(SubscriptionToken token) where T:CompositePresentationEvent<object>
        {
            EventManager.GetEvent<T>().Unsubscribe(token);
        }

        // shortcut methods for specific events
        //protected void SendScreenEvent(ScreenEventArgs sea)
        //{
        //    Send<ScreenEvent, ScreenEventArgs>(sea);
        //}
    }
}