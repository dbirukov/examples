using System;
using System.Net;
using Autofac;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using SDKClassicalLib.EventBus;
using SDKClassicalLib.Events;
using SDKClassicalLib.Interfaces;

namespace SDKClassicalESExample
{
    public class DependencyInjection : IDisposable
    {
        private IContainer Container { get; }

        public DependencyInjection()
        {
            var builder = new ContainerBuilder();

            IEventStoreConnection esconnection = GetConnection();
            esconnection.ConnectAsync();
            IEventBus bus = new EsEventBus(esconnection);

            builder.RegisterInstance(bus);
            
            var unsubGenericToken = bus.Subscribe<GenericEvent<int>>(OnIntEvent).Result; 
            var unsubTestEvent = bus.Subscribe<TestEventClass>(OnCustomEvent).Result; 
            
            Container = builder.Build();
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
        public T Resolve<T>(string name)
        {
            return Container.ResolveNamed<T>(name);
        }

        public void Dispose()
        {
            Container.Dispose();
        }
        
        private void OnCustomEvent(TestEventClass @event)
        {
            Console.WriteLine("Received: {0}", @event);
        }

        private void OnIntEvent(GenericEvent<int> intEvent)
        {
            Console.WriteLine("int event {0}", intEvent.Payload);
        }
        
        private IEventStoreConnection GetConnection()
        {
            var credentials = new UserCredentials("admin", "changeit");
            var connection =
                EventStoreConnection.Create(
                    ConnectionSettings.Create()
                        .UseConsoleLogger()
                        .SetDefaultUserCredentials(credentials),
                    new IPEndPoint(IPAddress.Loopback, 1113), "EventEmitter");
            return connection;
        }

    }
}