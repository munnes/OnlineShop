using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Models.Email;

namespace OnlineShop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IMailService mailService;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IMailService mailService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            this.mailService = mailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            public string FullName { get; set; }
        }
        //*********************

        public async Task SendEmail(string emailBody,string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            MailRequest mailRequest = new MailRequest();
            // mailRequest.Body = body + "<img src='https://www.countryflags.io/be/flat/64.png'>";  
            //mailRequest.Body = emailBody;
            mailRequest.Body = "Hi " + user.FullName + "; <br>" + emailBody +
             "<br/><br/>Best regards,<br>Queen Teem";
            mailRequest.ToEmail = email;
            mailRequest.Subject = "Reset Password";
            //**********************
            try
            {
            await   mailService.SendEmailAsync(mailRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //*************************
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (ModelState.IsValid)
            {
                  var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
              
                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                  var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);
         
                await SendEmail($"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.", Input.Email);
            
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
