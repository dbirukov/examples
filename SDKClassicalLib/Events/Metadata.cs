namespace SDKClassicalLib.Events
{
    /// <summary>
    /// Represents additional information about environment where event were generated (what service, instance, ...), 
    /// who issued event (user-id, request-id), information for deserialization, other information that matters for 
    /// reciever and infrastructure tools (logging, tracing, analitics)
    /// </summary>
    public class Metadata
    {
        public string EventType { get; protected set; }

        public Metadata(string eventType)
        {
            EventType = eventType;
        }
    }
}