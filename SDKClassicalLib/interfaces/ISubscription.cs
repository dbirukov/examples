using System;
using SDKClassicalLib.Events;

namespace SDKClassicalLib.Interfaces
{
    public interface ISubscription: IDisposable
    {
        SubscriptionToken SubscriptionToken { get; }
        void Publish(EventBase eventBase);
    }
}
