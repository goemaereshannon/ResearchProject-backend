using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class PropertyValue
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        public Guid PropertyId { get; set; }

        public Property Property { get; set; }
    }
}
