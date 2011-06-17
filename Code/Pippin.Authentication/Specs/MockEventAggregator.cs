using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Events;
using Pippin.UI.Events;
using MI.Model;

namespace MI.Authentication.Specs
{
    public class MockEventAggregator : IEventAggregator
    {
        public static Type ExpectedType = typeof(LogonEvent);

        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            if (ExpectedType == typeof(LogoffEvent))
                return new MockLogoffEvent() as TEventType;

            return new MockLogonEvent() as TEventType;
        }
    }

    
    public class MockLogonEvent : LogonEvent
    {
        public override void Publish(object payload)
        {
            EntryScreenSpec.LogonEventUser = (AppUser)payload;
        }
    }

    public class MockLogoffEvent : LogoffEvent
    {
        public override void Publish(object payload)
        {
        }
    }

}
