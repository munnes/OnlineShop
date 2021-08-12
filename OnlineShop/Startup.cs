using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Classes.Data;
using OnlineShop.Models;
using OnlineShop.Models.Email;
using OnlineShop.Models.Repositories;
using OnlineShop.ViewModel;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using OnlineShop.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OnlineShop
{
    public class Startup

    {
        private readonly IConfiguration configuration;
   
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            //*************************
            services.AddMvc();
            services.AddScoped<IOnlineShopRepository<Category>, CategoriesRepository>();
            services.AddScoped<IOnlineShopRepository<OnlineShop.Models.Product>, ProductsRepository>();
            services.AddScoped<IOnlineShopRepository<PrdByCat>, PrdByCatRepository>();
            services.AddScoped<IOnlineShopRepository<PrdInShop>, PrdInShopRepository>();
            services.AddScoped<IOnlineShopRepository<InShopView>, InShopViewRepository>();
            services.AddScoped<IOnlineShopRepository<Advertisment>, AdvertismentRepository>();
            services.AddScoped<IOnlineShopRepository<AvailStock>, AvailStockRepository>();
            services.AddScoped<IOnlineShopRepository<AvlProduct>, AvlProductRepository>();
            services.AddScoped<IOnlineShopRepository<AvlCategory>, AvlCategoryRepository>();
            services.AddScoped<IOnlineShopRepository<AdvertismentView>, AdvertismentViewRepository>();
            services.AddScoped<IOnlineShopRepository<Purchases>, PurchasesRepository>();
            services.AddScoped<IOnlineShopRepository<PrchFRecView>, PrchFRecViewRepository>();
            services.AddScoped<IOnlineShopRepository<UsrTrans>, UsrTransRepository>();
            services.AddScoped<IOnlineShopRepository<Orders>, OrdersRepository>();
            services.AddScoped<IOnlineShopRepository<CouriersView>, CouriersViewRepository>();
            services.AddScoped<IOnlineShopRepository<PurchaseView>, PurchaseViewRepository>();
            services.AddScoped<IOnlineShopRepository<Reviews>, ReviewsRepository>();
            services.AddScoped<IOnlineShopRepository<ReviewsView>, ReviewsViewRepository>();
            services.AddScoped<IOnlineShopRepository<SecondaryImg>, SecondaryImgRepository>();
            services.AddScoped<IOnlineShopRepository<SecondaryImgView>, SecondaryImgViewRepository>();

            services.AddDbContext<onlineShopContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlCon"));
            });
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, MailService>();


            // services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {

            //  StripeConfiguration.SetApiKey(configuration.GetSection("Stripe")["Secretkey"]);
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe")["Secretkey"];
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseStaticFiles();
            app.UseAuthentication();
            //  app.UseMvcWithDefaultRoute();
            app.UseMvc(route =>
            {

                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

            });
            //***********************

            CreateRolesAsync(serviceProvider).GetAwaiter().GetResult();
            CreateUsersAsync(serviceProvider).GetAwaiter().GetResult();

        }
        //*****************
        public async Task CreateRolesAsync(IServiceProvider serviceProvider)
        {
            var roles = new string[] { "Administrators", "Customers", "Couriers" };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }

            }
        }

        public async Task CreateUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager.Users.Where(u => u.Email == "admin@examplecompany.com").FirstOrDefault() == null)
            {
                var user = new ApplicationUser
                {
                    FullName="Admin",
                    Email = "admin@onlineShop.com",
                    UserName = "admin@onlineShop.com",
                };

                await userManager.CreateAsync(user, "Ad@123");
                await userManager.AddToRoleAsync(user, "Administrators");
            }
        }
        //******************************

    }
}

          
      