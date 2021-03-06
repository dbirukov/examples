﻿using System;
using System.Threading.Tasks;
using SDKClassicalLib.EventBus;
using SDKClassicalLib.Events;

namespace SDKClassicalESExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            var di = new DependencyInjection();
            
            IEventBus eventBus = di.Resolve<IEventBus>();
            
            await eventBus.Publish(new TestEventClass(Guid.NewGuid(), DateTime.Now)); // publishing custom user event

            await eventBus.Subscribe<GenericEvent<string>>(s =>
            {
                Console.WriteLine("String subscription 1: {0}", s.Payload);
            });
            var tokenTask = await eventBus.Subscribe<GenericEvent<string>>(s =>
            {
                Console.WriteLine("String subscription 2: {0}", s.Payload);
            });

            await eventBus.Publish(new GenericEvent<string>("Hello")); // publishing custom data 
            await eventBus.Publish(new GenericEvent<int>(123)); // publishing custom data
            await eventBus.Unsubscribe(tokenTask);
            
            await eventBus.Publish(new GenericEvent<string>("World")); // publishing custom data
            
            Console.WriteLine("->>Press key to stop waiting events");
            Console.ReadKey();
        }

    }
}