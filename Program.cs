using System;
using System.Reactive.Linq;
using System.Threading;

namespace ReactiveEventSDK
{
    class Customer
    {
        public Customer() { Orders = new ObservableCollection<Order>(); }
        public string CustomerName { get; set; }
        public string Region { get; set; }
        public ObservableCollection<Order> Orders { get; private set; }
    }

    class Order
    {
        public int OrderId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
    }

    static void Main()
    {
        var customers = new ObservableCollection<Customer>();

        var customerChanges = Observable.FromEventPattern(
            (EventHandler<NotifyCollectionChangedEventArgs> ev)
               => new NotifyCollectionChangedEventHandler(ev),
            ev => customers.CollectionChanged += ev,
            ev => customers.CollectionChanged -= ev);

        var watchForNewCustomersFromWashington =
            from c in customerChanges
            where c.EventArgs.Action == NotifyCollectionChangedAction.Add
            from cus in c.EventArgs.NewItems.Cast<Customer>().ToObservable()
            where cus.Region == "WA"
            select cus;

        Console.WriteLine("New customers from Washington and their orders:");

        watchForNewCustomersFromWashington.Subscribe(cus =>
        {
            Console.WriteLine("Customer {0}:", cus.CustomerName);

            foreach (var order in cus.Orders)
            {
                Console.WriteLine("Order {0}: {1}", order.OrderId, order.OrderDate);
            }
        });

        customers.Add(new Customer
        {
            CustomerName = "Lazy K Kountry Store",
            Region = "WA",
            Orders = { new Order { OrderDate = DateTimeOffset.Now, OrderId = 1 } }
        });

        Thread.Sleep(1000);
        customers.Add(new Customer
        {
            CustomerName = "Joe's Food Shop",
            Region = "NY",
            Orders = { new Order { OrderDate = DateTimeOffset.Now, OrderId = 2 } }
        });

        Thread.Sleep(1000);
        customers.Add(new Customer
        {
            CustomerName = "Trail's Head Gourmet Provisioners",
            Region = "WA",
            Orders = { new Order { OrderDate = DateTimeOffset.Now, OrderId = 3 } }
        });

        Console.ReadKey();
    }
}