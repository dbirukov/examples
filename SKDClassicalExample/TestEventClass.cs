using System;
using SDKClassicalLib.Events;

namespace SKDClassicalExample
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
    }
}