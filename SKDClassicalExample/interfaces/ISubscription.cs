using SKDClassicalExample.Events;

namespace SKDClassicalExample.Interfaces
{
    public interface ISubscription
    {
        SubscriptionToken SubscriptionToken { get; }
        void Publish(EventBase eventBase);
    }
}
