﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SDKClassicalLib;
using SDKClassicalLib.Events;
using SDKClassicalLib.Interfaces;

namespace SKDClassicalExample.EventBus
{
    public class EventBus : IEventBus
    {
        public EventBus()
        {
            _subscriptions = new Dictionary<Type, List<ISubscription>>();
        }

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

        private void PublishInternal<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
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

        public Task Publish<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            return PublishAsyncInternal(eventItem);
        }

        private async Task PublishAsyncInternal<TEventBase>(TEventBase eventItem) where TEventBase : EventBase
        {
            void PublishAction() => PublishInternal(eventItem);
            await Task.Run((Action) PublishAction);
        }

        private readonly Dictionary<Type, List<ISubscription>> _subscriptions;
    }
}