using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;

namespace SDKExample1
{
    public class DependencyInjection : IDisposable
    {
        public const string WA_NEW_CUSTOMER_STREAM = "WA-New-Customer-Stream";
        private IContainer Container { get; }

        public DependencyInjection()
        {
            var builder = new ContainerBuilder();

            var customerGenerator = CustomerGenerator();
            IObservable<NewCustomer> customerStream = customerGenerator.ToObservable();
            builder.RegisterInstance(customerStream).As<IObservable<NewCustomer>>();

            var watchForNewCustomersFromWA = 
                from c in customerGenerator.ToObservable()
                where c.Region == "WA"
                select c;
            
            builder.RegisterInstance(watchForNewCustomersFromWA)
                .Named<IObservable<NewCustomer>>(WA_NEW_CUSTOMER_STREAM);
            
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

        private IEnumerable<NewCustomer> CustomerGenerator()
        {
            while (true)
            {
                Thread.Sleep(800);

                yield return new NewCustomer
                {
                    CustomerName = Guid.NewGuid().ToString(),
                    Region = GetRandomState(),
                    Orders = {new Order {OrderDate = DateTimeOffset.Now, OrderId = _random.Next()}}
                };
            }
        }

        private static readonly string[] _states = {"WA", "NY", "CA"};
        private static readonly Random _random = new Random();
        
        private string GetRandomState()
        {
            return _states[_random.Next() % 3];
        }
    }
}