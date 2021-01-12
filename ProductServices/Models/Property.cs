using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class Property
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProductHasProperty> ProductHasProperties { get; set; }
        public ICollection<PropertyValue> PropertyValues { get; set; }
    }
}
