using System;
using System.Collections.Generic;
using System.Linq;
using SKDClassicalExample.Events;
using SKDClassicalExample.Interfaces;

namespace SKDClassicalExample
{
    public class EventBus : IEventBus
    {
        public EventBus()
        {
            _subscriptions = new Dictionary<Type, List<ISubscription>>();
        }

        public SubscriptionToken Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (!_subscriptions.ContainsKey(typeof(TEventBase)))
                _subscriptions.Add(typeof(TEventBase), new List<ISubscription>());

            var token = new SubscriptionToken(typeof(TEventBase));
            _subscriptions[typeof(TEventBase)].Add(new Subscription<TEventBase>(action, token));
            return token;
        }

        public void Unsubscribe(SubscriptionToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (!_subscriptions.ContainsKey(token.EventItemType)) return;
            
            var allSubscriptions = _subscriptions[token.EventItemType];
            var subscriptionToRemove =
                allSubscriptions.FirstOrDefault(x => x.SubscriptionToken.Token == token.Token);
            if (subscriptionToRemove != null)
                _subscriptions[token.EventItemType].Remove(subscriptionToRemove);
        }

        public void Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            if (eventItem == null)
                throw new ArgumentNullException(nameof(eventItem));

            var allSubscriptions = new List<ISubscription>();
            if (_subscriptions.ContainsKey(typeof(TEventBase)))
                allSubscriptions = _subscriptions[typeof(TEventBase)];

            foreach (var subscription in allSubscriptions)
            {
                try
                {
                    subscription.Publish(eventItem);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        public void PublishAsync<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            PublishAsyncInternal(eventItem, null);
        }

        public void PublishAsync<TEventBase>(TEventBase eventItem, AsyncCallback callback) where TEventBase : EventBase
        {
            PublishAsyncInternal(eventItem, callback);
        }

        private void PublishAsyncInternal<TEventBase>(TEventBase eventItem, AsyncCallback callback) where TEventBase : EventBase
        {
            Action publishAction = () => Publish(eventItem);
            publishAction.BeginInvoke(callback, null);
        }

        private readonly Dictionary<Type, List<ISubscription>> _subscriptions;
    }
}
