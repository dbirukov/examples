using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace SDKExample1
{
    class Program
    {
        public static void Main()
        {
            var di = new DependencyInjection();
            var watchForNewCustomersFromWA = di.Resolve<IObservable<NewCustomer>>();

            Console.WriteLine("New customers from Washington and their orders:");

            //subscription code should be used at bootstrap for webapps
            IDisposable subscription = watchForNewCustomersFromWA
                .SubscribeOn(Scheduler.Default) //run subscription in backgroung
                .Subscribe(newCaustomerHandler);

            Console.WriteLine("Press any key to unsubscribe");
            Console.ReadLine();

            subscription.Dispose();
        }

        private static void newCaustomerHandler(NewCustomer cus)
        {
            Console.Write("Handler: ");
            Console.WriteLine(cus.CustomerName);
        }
    }
}