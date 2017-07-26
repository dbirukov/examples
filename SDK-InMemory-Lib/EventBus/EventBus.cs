using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDKClassicalLib;
using SDKClassicalLib.EventBus;
using SDKClassicalLib.Events;

namespace SDK_InMemory_Lib.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<ISubscription>> _subscriptions = 
            new Dictionary<Type, List<ISubscription>>();
        
        public Task<SubscriptionToken> Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (!_subscriptions.ContainsKey(typeof(TEventBase)))
                _subscriptions.Add(typeof(TEventBase), new List<ISubscription>());

            var token = new SubscriptionToken(typeof(TEventBase));
            _subscriptions[typeof(TEventBase)].Add(new Subscription<TEventBase>(action, token));
            return Task.Run(() => token);
        }

        public Task Unsubscribe(SubscriptionToken token)
        {
            return Task.Run(() =>
            {

                if (token == null)
                    throw new ArgumentNullException(nameof(token));

                if (!_subscriptions.ContainsKey(token.EventItemType)) return;

                var allSubscriptions = _subscriptions[token.EventItemType];
                var subscriptionToRemove =
                    allSubscriptions.FirstOrDefault(x => x.SubscriptionToken.Token == token.Token);

                if (subscriptionToRemove != null)
                    _subscriptions[token.EventItemType].Remove(subscriptionToRemove);
            });
        }

        private async Task PublishInternal<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            if (eventItem == null)
                throw new ArgumentNullException(nameof(eventItem));

            var allSubscriptions = new List<ISubscription>();
            if (_subscriptions.ContainsKey(typeof(TEventBase)))
                allSubscriptions = _subscriptions[typeof(TEventBase)];

            await Task.WhenAll(allSubscriptions.Select(subscription => subscription.Publish(eventItem)));
        }

        public Task Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            return PublishInternal(eventItem);
        }
    }
}
