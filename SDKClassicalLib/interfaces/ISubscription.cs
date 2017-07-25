using SDKClassicalLib.Events;

namespace SDKClassicalLib.Interfaces
{
    public interface ISubscription
    {
        SubscriptionToken SubscriptionToken { get; }
        void Publish(EventBase eventBase);
    }
}
