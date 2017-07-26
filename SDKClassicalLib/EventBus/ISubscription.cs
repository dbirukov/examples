using System;
using System.Threading.Tasks;
using SDKClassicalLib.Events;

namespace SDKClassicalLib.EventBus
{
    /// <summary>
    /// Subscription object, that contains information about subscription
    /// </summary>
    public interface ISubscription: IDisposable
    {
        SubscriptionToken SubscriptionToken { get; }
        
        /// <summary>
        /// Publish event to subscription. In other words executes when particular event recieved
        /// </summary>
        /// <param name="event">Recieved event</param>
        /// <returns>Task which completes when event processed</returns>
        Task Publish(EventBase @event);
    }
}
