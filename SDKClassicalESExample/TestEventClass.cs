using System;
using SDKClassicalLib.Events;

namespace SDKClassicalESExample
{
    public class TestEventClass: EventBase
    {
        public Guid EventId { get; }
        public DateTime DateTime { get; }

        public TestEventClass(Guid eventId, DateTime dateTime)
        {
            EventId = eventId;
            DateTime = dateTime;
        }

        public override string ToString()
        {
            return "TestEventClass { EventId = " + EventId + " }";
        }
    }
}