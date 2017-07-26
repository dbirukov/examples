using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using SDKClassicalLib;
using SDKClassicalLib.EventBus;
using SDKClassicalLib.Events;

namespace SDK_EventStore_Lib
{
    internal class EsSubscription<TEventBase> : ISubscription where TEventBase : EventBase
    {
        public SubscriptionToken SubscriptionToken { get; }

        public EventStoreSubscription EventStoreSubscription
        {
            get; 
            set; //todo refactor
        }

        public EsSubscription(Action<TEventBase> action, SubscriptionToken token)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            SubscriptionToken = token ?? throw new ArgumentNullException(nameof(token));
        }

        public async Task Publish(EventBase eventItem)
        {
            if (!(eventItem is TEventBase))
            {
                throw new ArgumentException("Event Item is not the correct type.");
            }

            await Task.Run(() => _action.Invoke((TEventBase) eventItem));
        }

        public void Dispose()
        {
            EventStoreSubscription?.Dispose();
        }
        
        private readonly Action<TEventBase> _action;
    }
}
