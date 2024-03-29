﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class PriceDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public double Value { get; set; }
        public string Currency { get; set; } = "€"; 
        public float? Discount { get; set; }

    }
}
