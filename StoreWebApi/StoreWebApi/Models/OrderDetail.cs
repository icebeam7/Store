using System;
using System.Collections.Generic;

namespace StoreWebApi.Models
{
    public partial class OrderDetail
    {
        public int CustomerOrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }

        public CustomerOrder CustomerOrder { get; set; }
        public Product Product { get; set; }
    }
}
