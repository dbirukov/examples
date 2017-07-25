using System;
using SDKExample1;
using SDKClassicalLib.Events;
using SDKClassicalLib.Interfaces;

namespace SKDClassicalExample
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var di = new DependencyInjection();
            
            IEventBus eventBus = di.Resolve<IEventBus>();
            
            eventBus.Publish(new TestEventClass(Guid.NewGuid())).Wait(); // publishing custom user event

            eventBus.Subscribe<GenericEvent<string>>(s =>
            {
                Console.WriteLine("String subscription 1: {0}", s.Payload);
            });
            var tokenTask = eventBus.Subscribe<GenericEvent<string>>(s =>
            {
                Console.WriteLine("String subscription 2: {0}", s.Payload);
            }).Result;

            eventBus.Publish(new GenericEvent<string>("Hello")).Wait(); // publishing custom data 
            eventBus.Publish(new GenericEvent<int>(123)).Wait(); // publishing custom data
            eventBus.Unsubscribe(tokenTask).Wait();
            
            eventBus.Publish(new GenericEvent<string>("World")).Wait(); // publishing custom data
            
            Console.WriteLine("->>Press key to stop waiting events");
            Console.ReadKey();
        }
    }
}