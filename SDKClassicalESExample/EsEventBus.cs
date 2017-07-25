using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using SDKClassicalLib;
using SDKClassicalLib.Events;
using SDKClassicalLib.Interfaces;

namespace SDKClassicalESExample
{
    public class EsEventBus : IEventBus
    {
        private const string EsExample = "es-example-";
        private readonly IEventStoreConnection _connection;
        private readonly Dictionary<Type, List<ISubscription>> _subscriptions = new Dictionary<Type, List<ISubscription>>();

        public EsEventBus(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        public async Task<SubscriptionToken> Subscribe<TEventBase>(Action<TEventBase> action) where TEventBase : EventBase
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (!_subscriptions.ContainsKey(typeof(TEventBase)))
                _subscriptions.Add(typeof(TEventBase), new List<ISubscription>());

            var token = new SubscriptionToken(typeof(TEventBase));
            var esSubscription = new EsSubscription<TEventBase>(action, token);
            _subscriptions[typeof(TEventBase)].Add(esSubscription);

            await _connection.SubscribeToStreamAsync(EsExample + typeof(TEventBase) ,false, EventAppeared(esSubscription), SubscriptionDropped);
            return token;
        }

        public Task Unsubscribe(SubscriptionToken token)
        {
            throw new NotImplementedException();
        }

        public Task Publish<TEventBase>(TEventBase @event) where TEventBase : EventBase
        {
            return EmitEvent(EsExample + typeof(TEventBase), @event);
        }
        
        private Task<WriteResult> EmitEvent<TEventBase>(string stream, TEventBase @event) where TEventBase : EventBase
        {
            //todo add retries in case of error 
            Console.WriteLine("Sending to stream {0}: event {1}", stream, @event);
            return _connection.AppendToStreamAsync(stream, ExpectedVersion.Any, CreateEvent(@event));
        }

        private EventData[] CreateEvent<TEventBase>(TEventBase @event)  where TEventBase : EventBase
        {
            return new[]
            {
                new EventData(Guid.NewGuid(), typeof(TEventBase).ToString(), true,
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)), null)
            };
        }
        
        private Action<EventStoreSubscription, ResolvedEvent> EventAppeared<TEventBase>(EsSubscription<TEventBase> esSubscription) where TEventBase : EventBase
        {
            return (subscription, resolvedEvent) =>
            {
                try
                {
                    var @event = EventDeserializer.Deserialize<TEventBase>(resolvedEvent);
                    if (@event != null) esSubscription.Publish(@event);
                }
                catch (Exception e)
                {
                    //todo 
                    Console.WriteLine("EventAppeared OnError: {0}", e);
                }
            };
        }

        private void SubscriptionDropped(EventStoreSubscription subscription, SubscriptionDropReason dropReason, Exception e)
        {
            if (e != null)
            {
                Console.WriteLine("SubscriptionDropped OnError: {0}", e);
            }
            else
            {
                Console.WriteLine("SubscriptionDropped OnCompleted: {0}", e);
            }
        }


    }
}