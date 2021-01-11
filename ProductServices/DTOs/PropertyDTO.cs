using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.DTOs
{
    public class PropertyDTO
    {
        public string Name { get; set; }
        public ICollection<PropertyValueDTO> PropertyValues { get; set; }
    }
}
