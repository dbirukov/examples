﻿using System;
using System.Threading.Tasks;
using SDKClassicalLib;
using SDKClassicalLib.Events;
using SDKClassicalLib.Interfaces;

namespace SKDClassicalExample.EventBus
{
    internal class Subscription<TEventBase> : ISubscription where TEventBase : EventBase
    {
        public SubscriptionToken SubscriptionToken { get; }

        public Subscription(Action<TEventBase> action, SubscriptionToken token)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            SubscriptionToken = token ?? throw new ArgumentNullException(nameof(token));
        }

        public Task Publish(EventBase eventItem)
        {
            if (!(eventItem is TEventBase))
            {
                throw new ArgumentException("Event Item is not the correct type.");
            }

            return Task.Run(() => _action.Invoke((TEventBase) eventItem));
        }

        public void Dispose()
        {
        }
        
        private readonly Action<TEventBase> _action;
    }
}
