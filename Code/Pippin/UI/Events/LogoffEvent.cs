using Microsoft.Practices.Prism.Events;

namespace Pippin.UI.Events
{
    public class LogoffEvent : CompositePresentationEvent<object>
    {
    }

    public static class EventAggregator_extensions_for_LogoffEvent
    {
        public static void Send_Logoff(this IEventAggregator ea)
        {
            ea.GetEvent<LogoffEvent>().Publish(null);
        }
    }
}