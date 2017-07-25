using System.Collections.Generic;

namespace SDKExample1
{
    public class NewCustomer
    {
        public NewCustomer()
        {
            Orders = new List<Order>();
        }

        public string CustomerName { get; set; }
        public string Region { get; set; }
        public List<Order> Orders { get; }

    }
}