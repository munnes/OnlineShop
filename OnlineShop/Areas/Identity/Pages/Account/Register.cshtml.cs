using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OnlineShop.Models;
using OnlineShop.Models.Email;

namespace OnlineShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
   // [Authorize (Roles = "Administrators")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMailService mailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationUser user;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IMailService mailService,
            RoleManager<IdentityRole> roleManager)  
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.mailService = mailService;
            _roleManager = roleManager;
        }

       
       [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

       
        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name ="Role")] 
            public string Name { get; set; }
         
        }

        /*  public void OnGet(string returnUrl = null)
          {
              ReturnUrl = returnUrl;
          }*/
        public async Task SendEmail(string emailBody, string email)
        {
            MailRequest mailRequest = new MailRequest();
            var user = await _userManager.FindByEmailAsync(email);
            // mailRequest.Body = body + "<img src='https://www.countryflags.io/be/flat/64.png'>";  
            mailRequest.Body = "Hi " + user.FullName + "; <br> Welcome to Queen, "+emailBody+
                "<br/><br/>Best regards,<br>Queen Teem";
            mailRequest.ToEmail = email;
            mailRequest.Subject = "Confirm your email";
            //**********************
            try
            {
              await  mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
           /* var userId = _userManager.GetUserId(HttpContext.User);
            ApplicationUser curUser = _userManager.FindByIdAsync(userId).Result;
            ViewData["me"] = curUser.Id;
              string rolename = await _userManager.GetRolesAsync(curUser).FirstOrDefault();
            var roles = _userManager.GetRolesAsync(curUser);*/

            ViewData["isAd"] =(bool) HttpContext.User.IsInRole("Administrators");
            ViewData["roles"] = _roleManager.Roles.ToList();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            bool isAministrator= (bool)HttpContext.User.IsInRole("Administrators");
            ViewData["isAd"] = isAministrator;
            ViewData["roles"] = _roleManager.Roles.ToList();
            IdentityRole role=new IdentityRole() ;

            returnUrl = returnUrl ?? Url.Content("~/");
            if (isAministrator)
            { role = _roleManager.FindByIdAsync(Input.Name).Result; }
            else
            {
             role = _roleManager.FindByNameAsync(Input.Name).Result;
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FullName = Input.FullName };
                var result = await _userManager.CreateAsync(user, Input.Password);
            
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, role.Name);
                  
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                  //  code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);
                 await  SendEmail($"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.", Input.Email);

                    /*   await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                           $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");*/
                    if (isAministrator)
                    {
                        return RedirectToAction("RegisterViewModel", "Home");
                    }
                    else { 
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        

            return Page();
        }
    }
}
