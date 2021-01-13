using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServices.DTOs
{
    public class RegisterDTO
    {
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public int HouseNumber { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
