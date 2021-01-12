using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServices.Models
{
    public class Role: IdentityRole<Guid>
    {
        public Role() : base() { }
        public Role(string name) : base(name) { } //voor aanmaken op naam in Seeder
        public string Description { get; set; }
        public override Guid Id { get; set; } = Guid.NewGuid();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
