using System;
using System.Collections.Generic;

namespace StoreWebApi.Models
{
    public partial class CustomerOrder
    {
        public CustomerOrder()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public int OrderStatusId { get; set; }
        public decimal Amount { get; set; }

        public Customer Customer { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
