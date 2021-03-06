﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using SDKClassicalLib;
using SDKClassicalLib.EventBus;
using SDKClassicalLib.Events;
using SDK_EventStore_Lib.Deserialization;
using SDK_EventStore_Lib.Extentions;

namespace SDK_EventStore_Lib.EventBus
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
            var eventStoreSubscription = 
                await _connection.SubscribeToStreamAsync(GetStreamName<TEventBase>() ,false, EventAppeared(esSubscription), SubscriptionDropped);
            
            //todo refactor to constructor injection
            esSubscription.EventStoreSubscription = eventStoreSubscription;
            
            _subscriptions[typeof(TEventBase)].Add(esSubscription);
            
            return token;
        }

        public async Task Unsubscribe(SubscriptionToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (!_subscriptions.ContainsKey(token.EventItemType))
            {
                return;
            }

            await Task.Run(() =>
            {
                var allSubscriptions = _subscriptions[token.EventItemType];
                var subscriptionToRemove =
                    allSubscriptions.FirstOrDefault(x => x.SubscriptionToken.Token == token.Token);

                if (subscriptionToRemove == null)
                {
                    return;
                }
                
                _subscriptions[token.EventItemType].Remove(subscriptionToRemove);
                subscriptionToRemove.Dispose();
            });
        }

        public Task Publish<TEventBase>(TEventBase @event) where TEventBase : EventBase
        {
            return EmitEvent(GetStreamName<TEventBase>(), @event);
        }

        private static string GetStreamName<TEventBase>() where TEventBase : EventBase
        {
            //simple naming strategy, in microservice env should be applied more complex strategy
            return EsExample + typeof(TEventBase);
        }

        private Task<WriteResult> EmitEvent<TEventBase>(string stream, TEventBase @event) where TEventBase : EventBase
        {
            Console.WriteLine("Sending to stream {0}: event {1}", stream, @event);
            return _connection
                .AppendToStreamAsync(stream, ExpectedVersion.Any, CreateEvent(@event))
                .Retry(5, 200);
        }

        private EventData[] CreateEvent<TEventBase>(TEventBase @event)  where TEventBase : EventBase
        {
            var metadata = new Metadata(typeof(TEventBase).AssemblyQualifiedName);
            return new[]
            {
                new EventData(Guid.NewGuid(), typeof(TEventBase).ToString(), true,
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)), 
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(metadata)))
            };
        }
        
        private Action<EventStoreSubscription, ResolvedEvent> EventAppeared<TEventBase>(EsSubscription<TEventBase> esSubscription) where TEventBase : EventBase
        {
            return async (subscription, resolvedEvent) =>
            {
                try
                {
                    var @event = EventDeserializer.Deserialize<TEventBase>(resolvedEvent);
                    if (@event != null)
                    {
                        await esSubscription.Publish(@event);
                    }
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
                Console.WriteLine("SubscriptionDropped without errors");
            }
        }
    }
}