using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebsiteV3.Services;

namespace WebsiteV3.Areas.Home.Pages
{
    [AllowAnonymous]
    public class ContactMeModel : PageModel
    {
        private readonly ILogger<ContactMeModel> _logger;

        public ContactMeModel(IOptions<AuthMessageSenderOptions> optionsAccessor,           
            ILogger<ContactMeModel> logger)
        {
            _logger = logger;
            Options = optionsAccessor.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        
        public class InputModel
        {
            [Required]
            [StringLength(50)]
            public string Name { get; set; }
            [Required]
            [StringLength(255)]
            [EmailAddress]
            public string Email { get; set; }
            public string Subject { get; set; }
            [Required]
            [StringLength(1000)]
            public string Message { get; set; }
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "The Privacy Consent must be confirmed")]
            [Display(Name = "I consent to the website storing my name and email address so they can respond to my query.")]
            public bool PrivacyConsent { get; set; }
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        //--------This is for recaptcha if I want to implement this later --------//
        //private bool RecaptchaPassed(string recaptchaResponse)
        //{
        //    _logger.LogDebug("Contact.RecaptchaPassed entered");

        //    var secret =
        //        _configuration.GetSection("RecaptchaKey").Value;

        //    var endPoint =
        //        _configuration.GetSection("RecaptchaEndPoint").Value;

        //    var googleCheckUrl =
        //        $"{endPoint}?secret={secret}&response={recaptchaResponse}";

        //    _logger.LogDebug("Checking reCaptcha");
        //    var httpClient = _httpClientFactory.CreateClient();

        //    var response = httpClient.GetAsync(googleCheckUrl).Result;

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        _logger.LogDebug($"reCaptcha bad response {response.StatusCode}");
        //        return false;
        //    }

        //    dynamic jsonData =
        //        JObject.Parse(response.Content.ReadAsStringAsync().Result);

        //    _logger.LogDebug("reCaptcha returned successfully");

        //    return (jsonData.success == "true");
        //}
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogDebug("ContactMe.OnPostSync entered");

            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Model state not valid");
                return Page();
            }

            //---------This is for recaptcha if I want to implement this later ----------//
            //var gRecaptchaResponse = Request.Form["g-recaptcha-response"];

            //if (string.IsNullOrEmpty(gRecaptchaResponse)
            //    || !RecaptchaPassed(gRecaptchaResponse))
            //{
            //    _logger.LogDebug("Recaptcha empty or failed");
            //    ModelState.AddModelError(string.Empty, "You failed the CAPTCHA");
            //    return Page();
            //}

            // Mail header
            var from = new EmailAddress(
                "krystalruwoldt@bigpond.com", "germistry");
            var to = new EmailAddress(
                Options.ContactMeMailbox, Options.SendGridUser);
            string subject = Input.Subject;
            string message = $"Contact Me Email From {Input.Email} - {Input.Name}. Message Body: {Input.Message}";
            
            // Get SendGrid client ready
            var client = new SendGridClient(Options.SendGridKey);

            var msg = MailHelper.CreateSingleEmail(from, to, subject,
                message, WebUtility.HtmlEncode(message));

            _logger.LogDebug("Sending email via SendGrid");
            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                _logger.LogDebug($"Sendgrid problem {response.StatusCode}");
                throw new ExternalException("Error sending message");
            }

            // On success go to contact result result which is a thankyou page
            _logger.LogDebug("Email sent via SendGrid");
            return RedirectToPage("ContactResult");
        }
    }
}