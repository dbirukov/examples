namespace SKDClassicalExample.Events
{
    public class GenericEvent<TPayload> : EventBase
    {
        public TPayload Payload { get; protected set; }

        public GenericEvent(TPayload payload)
        {
            Payload = payload;
        }
    }
}
