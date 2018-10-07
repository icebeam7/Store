using System;
using System.Collections.Generic;

namespace StoreWebApi.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerOrder = new HashSet<CustomerOrder>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ICollection<CustomerOrder> CustomerOrder { get; set; }
    }
}
