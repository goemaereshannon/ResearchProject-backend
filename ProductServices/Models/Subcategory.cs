using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Models
{
    public class Subcategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
