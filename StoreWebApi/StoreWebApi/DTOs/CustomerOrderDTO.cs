using System;
using System.Collections.Generic;

namespace StoreWebApi.DTOs
{
    public class CustomerOrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public int OrderStatusId { get; set; }
        public decimal Amount { get; set; }

        public CustomerDTO Customer { get; set; }
        public OrderStatusDTO OrderStatus { get; set; }
        public List<OrderDetailDTO> OrderDetail { get; set; }
    }
}
