using IdentityServices.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServices.Data
{
    public class ApplicationDbContextSeeder
    {
        private readonly ApplicationDbContext context;

        public static List<string> EmailList = new List<string>
        {
            "shannon.goemaere@gmail.com"
        };

        public static async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            try
            {
                //inlezen van setup
                var contenRoot = env.ContentRootPath;
                var webroot = env.WebRootPath;


                await SeedRoles(roleManager);
                await context.SaveChangesAsync();
                await SeedAdmins(userManager, roleManager);
               // await SeedDocent(userManager, roleManager);
                await SeedCustomers(userManager, roleManager);
                await context.SaveChangesAsync();

            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
                throw;
            }
        }


        private static async Task SeedCustomers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {

            try
            {
                foreach (string email in EmailList)
                {
                    if (userManager.FindByNameAsync(email).Result == null)
                    {
                        User customer = new User
                        {
                            Id = Guid.NewGuid(),

                            Email = email,
                            UserName = email,
                            FirstName = email.Split('.')[0],
                            LastName = email.Split('.')[1].Split('@')[0],
                            StreetName = "Graaf Karel De Goedelaan",
                            HouseNumber = 6,
                            PostCode = "8850",
                            City = "Kortrijk"



                        };
                        await userManager.CreateAsync(customer, "Wachtwoord@0");
                        var role = roleManager.Roles.Where(r => r.Name.StartsWith("C")).FirstOrDefault();
                        var userResult = await userManager.AddToRoleAsync(customer, role.Name);

                        if (!userResult.Succeeded)
                        {
                            throw new InvalidOperationException("Failed to build user and roles");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
        }

        private static async Task SeedAdmins(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            try
            {
                if (userManager.FindByEmailAsync("shannon.goemaere@student.howest.be").Result == null)
                {
                    User admin = new User
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Shannon", 
                        LastName = "Goemaere", 
                        Email = "shannon.goemaere@student.howest.be",
                        UserName = "ShannonGoemaere",
                        StreetName = "Graaf Karel De Goedelaan",
                        HouseNumber = 5,
                        PostCode = "8850",
                        City = "Kortrijk"
                    };
                    await userManager.CreateAsync(admin, "Wachtwoord@0");
                    var role = roleManager.Roles.Where(r => r.Name.StartsWith("A")).FirstOrDefault();
                    var userResult = await userManager.AddToRoleAsync(admin, role.Name);
                    if (!userResult.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to build user and roles");
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        private static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            string[] roleNames = { "Admin", "Customer", "Visitor" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new Role(roleName)); //role aanmaken via constructor van ons eigen role model

                }
            }
        }
    }
}
