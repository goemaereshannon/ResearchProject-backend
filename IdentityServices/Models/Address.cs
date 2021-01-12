using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServices.Models
{
    public class Address
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        //public string Country { get; set; }
    }
}
