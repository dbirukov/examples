using System;
using Autofac;
using SDKClassicalLib;
using SDKClassicalLib.Events;
using SDKClassicalLib.Interfaces;
using SKDClassicalExample;

namespace SDKExample1
{
    public class DependencyInjection : IDisposable
    {
        private IContainer Container { get; }

        public DependencyInjection()
        {
            var builder = new ContainerBuilder();

            IEventBus bus = new EventBus();

            builder.RegisterInstance(bus);
            
            var unsubGenericToken = bus.Subscribe<GenericEvent<int>>(OnIntEvent); 
            var unsubTestEvent = bus.Subscribe<TestEventClass>(OnCustomEvent); 
            
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
        
        private void OnCustomEvent(TestEventClass customEvent)
        {
            Console.WriteLine("Received TestEventClass");
        }

        private void OnIntEvent(GenericEvent<int> intEvent)
        {
            Console.WriteLine("int event {0}", intEvent.Payload);
        }
    }
}