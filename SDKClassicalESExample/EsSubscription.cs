﻿using System;
using EventStore.ClientAPI;
using SDKClassicalLib;
using SDKClassicalLib.Events;
using SDKClassicalLib.Interfaces;

namespace SDKClassicalESExample
{
    internal class EsSubscription<TEventBase> : ISubscription where TEventBase : EventBase
    {
        public SubscriptionToken SubscriptionToken { get; }

        public EventStoreSubscription EventStoreSubscription
        {
            get; 
            set; //todo remove
        }

        public EsSubscription(Action<TEventBase> action, SubscriptionToken token)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            SubscriptionToken = token ?? throw new ArgumentNullException(nameof(token));
        }


        public void Publish(EventBase eventItem)
        {
            if (!(eventItem is TEventBase))
                throw new ArgumentException("Event Item is not the correct type.");

            _action.Invoke((TEventBase) eventItem);
        }

        private readonly Action<TEventBase> _action;

        public void Dispose()
        {
            EventStoreSubscription?.Dispose();
        }
    }
}
