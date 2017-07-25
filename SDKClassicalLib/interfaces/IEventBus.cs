using System;
using System.Threading.Tasks;
using SDKClassicalLib.Events;

namespace SDKClassicalLib.Interfaces
{
    public interface IEventBus
    {
        SubscriptionToken Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase;
        void Unsubscribe(SubscriptionToken token);
        void Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase;
        Task PublishAsync<TEventBase>(TEventBase eventItem) where TEventBase : EventBase;
    }
}
