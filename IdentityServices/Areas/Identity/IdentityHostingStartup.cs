using IdentityServices.Data;
using IdentityServices.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[assembly: HostingStartup(typeof(IdentityServices.Areas.Identity.IdentityHostingStartup))]
namespace IdentityServices.Areas.Identity
{

    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDefaultIdentity<User>(o => o.SignIn.RequireConfirmedAccount = false).AddRoles<Role>().AddEntityFrameworkStores<ApplicationDbContext>();
            });
        }
    }
}
