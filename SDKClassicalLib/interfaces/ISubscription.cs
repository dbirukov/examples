using System;
using System.Threading.Tasks;
using SDKClassicalLib.Events;

namespace SDKClassicalLib.Interfaces
{
    public interface ISubscription: IDisposable
    {
        SubscriptionToken SubscriptionToken { get; }
        Task Publish(EventBase eventBase);
    }
}
