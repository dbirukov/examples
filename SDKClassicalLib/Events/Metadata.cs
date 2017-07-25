namespace SDKClassicalLib.Events
{
    public class Metadata
    {
        public string EventType { get; protected set; }

        public Metadata(string eventType)
        {
            EventType = eventType;
        }
    }
}