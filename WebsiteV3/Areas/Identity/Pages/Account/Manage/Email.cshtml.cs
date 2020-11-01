using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WebsiteV3.Models;
using NETCore.MailKit.Core;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace WebsiteV3.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly string _templatesPath;

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService, IConfiguration pathConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _templatesPath = pathConfig["Path:Templates"];
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "New email")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var emailExists = await _userManager.FindByEmailAsync(Input.NewEmail);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (emailExists != null)
            {
                StatusMessage = "Not valid. Please enter a valid email address.";
                return RedirectToPage();
            }
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = Input.NewEmail, code = code },
                    protocol: Request.Scheme);
                var body = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>. <br />If you did not sign up for this website, please DO NOT confirm your email, instead please notify us by replying to this email so any security breach can be investigated.";
                string path = Path.Combine(_templatesPath);
                string template = "IdentityTemplate.html";
                string FilePath = Path.Combine(path, template);

                StreamReader str = new StreamReader(FilePath);
                string mailText = str.ReadToEnd();
                str.Close();
                mailText = mailText.Replace("[username]", user.UserName).Replace("[body]", body);
                var subject = "Confirm your change of email for germistry aka Krystal Ruwoldt's Portfolio and Blog";
               
                await _emailService.SendAsync(Input.NewEmail, subject, mailText, true);

                StatusMessage = "Confirmation link to change email sent. Please check your email.";
                return RedirectToPage();
            }

            StatusMessage = "Your email is unchanged.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            var body = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>. <br />If you did not sign up for this website, please DO NOT confirm your email, instead please notify us by replying to this email so any security breach can be investigated.";
            string path = Path.Combine(_templatesPath);
            string template = "IdentityTemplate.html";
            string FilePath = Path.Combine(path, template);

            StreamReader str = new StreamReader(FilePath);
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[username]", user.UserName).Replace("[body]", body);
            var subject = "Confirm your account for germistry aka Krystal Ruwoldt's Portfolio and Blog";

            await _emailService.SendAsync(user.Email, subject, mailText, true);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
