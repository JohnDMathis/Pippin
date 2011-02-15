using Microsoft.Practices.Prism.Events;

namespace Pippin.UI.Events
{
    public class LogonEvent : CompositePresentationEvent<object>
    {
    }

    public static class EventAggregator_extensions_for_LogonEvent
    {
        public static void Send_Logon(this IEventAggregator ea, object payload)
        {
            ea.GetEvent<LogonEvent>().Publish(payload);
        }
    }

}