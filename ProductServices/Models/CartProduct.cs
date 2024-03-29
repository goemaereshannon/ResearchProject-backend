﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class CartProduct
    { 
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public Guid ProductId { get; set; }
        public Guid CartId { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public int ItemQuantity { get; set; } = 1; 

    }
}
