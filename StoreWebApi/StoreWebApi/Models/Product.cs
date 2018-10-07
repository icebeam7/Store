using System;
using System.Collections.Generic;

namespace StoreWebApi.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
