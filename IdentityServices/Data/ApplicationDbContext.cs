using System;
using System.Collections.Generic;
using System.Text;
using IdentityServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServices.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasMany(e => e.UserRoles)
                .WithOne(entity => entity.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasMany(e => e.UserRoles)
                .WithOne(entity => entity.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            });

        }
    }
}
