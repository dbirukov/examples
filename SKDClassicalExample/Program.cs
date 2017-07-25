﻿using System;
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
            
            eventBus.Publish(new TestEventClass(Guid.NewGuid()));
  
            eventBus.Subscribe<GenericEvent<string>>(s =>
            {
                Console.WriteLine(s.Payload);
            });
  
            eventBus.Publish(new GenericEvent<string>("Hello"));
            eventBus.PublishAsync(new GenericEvent<int>(123)).Wait();
        }
    }
}