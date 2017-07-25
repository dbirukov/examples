using System;
using SDKClassicalLib.Events;

namespace SKDClassicalExample
{
    public class TestEventClass: EventBase
    {
        public Guid EventId { get; }

        public TestEventClass(Guid eventId)
        {
            EventId = eventId;
        }
    }
}