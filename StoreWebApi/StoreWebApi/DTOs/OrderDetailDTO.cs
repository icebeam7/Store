using System;
using System.Collections.Generic;

namespace StoreWebApi.DTOs
{
    public class OrderDetailDTO
    {
        public int CustomerOrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }

        public CustomerOrderDTO CustomerOrder { get; set; }
        public ProductDTO Product { get; set; }
    }
}
