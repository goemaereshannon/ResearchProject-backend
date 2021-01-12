using Microsoft.AspNetCore.Identity;
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
        public string LastName { get; set; }


        [Required]
        public Address Address { get; set; }

        // Niet <IdentityUserRole> !
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
