﻿using System;
using System.Collections.Generic;

namespace MyStore.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }

        public List<OrderDetailDTO> OrderDetail { get; set; }
    }
}
