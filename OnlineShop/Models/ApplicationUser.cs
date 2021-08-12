using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace OnlineShop.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public string Location { get; set; }

        //*****************************

        /*  public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
             {
                 // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
                 var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
                 // Add custom user claims here
                 userIdentity.AddClaim(new Claim("FullName", FullName));
                 return userIdentity;
             }*/

        //************************************
    }
}
