using System;
using System.Threading.Tasks;
using SDKClassicalLib.Events;

namespace SDKClassicalLib.EventBus
{
    public interface IEventBus
    {
        Task<SubscriptionToken> Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase;
        Task Unsubscribe(SubscriptionToken token);
        Task Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase;
    }
}
