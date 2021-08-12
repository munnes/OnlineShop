using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Data;
using OnlineShop.Models;

[assembly: HostingStartup(typeof(OnlineShop.Areas.Identity.IdentityHostingStartup))]
namespace OnlineShop.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<OnlineShopContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SqlCon")));
                services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
                 .AddDefaultUI()
                 .AddEntityFrameworkStores<OnlineShopContext>()
                 .AddDefaultTokenProviders();
                
/*
                 services.AddDefaultIdentity<ApplicationUser>()
                       .AddEntityFrameworkStores<OnlineShopContext>();*/
            });
        }
    }
}