using System;
using System.Threading.Tasks;
using SDKClassicalLib.Events;

namespace SDKClassicalLib.EventBus
{
    /// <summary>
    /// Used to publish events to subscriber. Event published to the system should be delivered to all subscribers.
    /// </summary>
    public interface IEventBus
    {
        
        /// <summary>
        /// Publish new event to the bus
        /// </summary>
        /// <param name="event">Event to publish. Must be serializable</param>
        /// <typeparam name="TEventBase">Type of event to publish</typeparam>
        /// <returns>Task which completes when event delivered</returns>
        Task Publish<TEventBase>(TEventBase @event) where TEventBase : EventBase;

        
        /// <summary>
        /// Add subscriber for particular event type. It is possible to create one or more subscriptions 
        /// </summary>
        /// <param name="action">Action that react if event revieved</param>
        /// <typeparam name="TEventBase">Type of the revieved event</typeparam>
        /// <returns>Task with Subscription token, that could be used to unsubscribe</returns>
        Task<SubscriptionToken> Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase;
        
        /// <summary>
        /// Unsubscribe subscriber by token
        /// </summary>
        /// <param name="token">Subscription token recivied when subscription was created</param>
        /// <returns>Task which completes then subscription canceled</returns>
        Task Unsubscribe(SubscriptionToken token);
    }
}
