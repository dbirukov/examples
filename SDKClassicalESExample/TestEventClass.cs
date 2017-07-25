using System;
using SDKClassicalLib.Events;

namespace SDKClassicalESExample
{
    public class TestEventClass: EventBase
    {
        public Guid EventId { get; }

        public TestEventClass(Guid eventId)
        {
            EventId = eventId;
        }

        public override string ToString()
        {
            return "TestEventClass { EventId = " + EventId + " }";
        }
    }
}