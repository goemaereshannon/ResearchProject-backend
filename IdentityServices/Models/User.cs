﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServices.Models
{
    public class User:IdentityUser<Guid>
    {
        public override Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }


        [Required]
        public string StreetName { get; set; }
        [Required]
        public int HouseNumber { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string City { get; set; }
        //public string Country { get; set; }

        // Niet <IdentityUserRole> !
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
