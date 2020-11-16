using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WebsiteV3.Models;
using NETCore.MailKit.Core;
using Microsoft.Extensions.Configuration;
using WebsiteV3.Helpers;

namespace WebsiteV3.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly string _templatesPath;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailService emailService, IConfiguration pathConfig)
        {
            _userManager = userManager;
            _emailService = emailService;
            _templatesPath = pathConfig["Path:Templates"];
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

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

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);
                
                var body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>. <br />If you did not sign up for this website, please DO NOT reset your password, instead please notify us by replying to this email so any security breach can be investigated.";
                string mailText = EmailHelper.BuildTemplate(_templatesPath, "IdentityTemplate.html");
                mailText = mailText.Replace("[username]", user.UserName).Replace("[body]", body);
                var subject = "Reset your password for germistry aka Krystal Ruwoldt's Portfolio and Blog";

                await _emailService.SendAsync(user.Email, subject, mailText, true);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
