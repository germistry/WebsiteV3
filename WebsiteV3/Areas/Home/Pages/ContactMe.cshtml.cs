using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using NETCore.MailKit.Core;
using Microsoft.Extensions.Configuration;
using System.IO;
using GoogleReCaptcha.V3.Interface;

namespace WebsiteV3.Areas.Home.Pages
{
    [AllowAnonymous]
    public class ContactMeModel : PageModel
    {
        private readonly ILogger<ContactMeModel> _logger;
        private readonly IEmailService _emailService;
        private readonly string _templatesPath;
        private readonly ICaptchaValidator _captchaValidator;

        public ContactMeModel(IEmailService emailService,
            ILogger<ContactMeModel> logger, 
            IConfiguration pathConfig, 
            ICaptchaValidator captchaValidator)
        {
            _logger = logger;
            _emailService = emailService;
            _templatesPath = pathConfig["Path:Templates"];
            _captchaValidator = captchaValidator;
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
            public string Message { get; set; }
            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "The Privacy Consent must be confirmed")]
            [Display(Name = " I consent to the website storing my name and email address so they can respond to my query.")]
            public bool PrivacyConsent { get; set; }
        }
        
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
        public async Task<IActionResult> OnPostAsync(string captcha)
        {
            _logger.LogDebug("ContactMe.OnPostSync entered");

            if (!await _captchaValidator.IsCaptchaPassedAsync(captcha))
            {
                ModelState.AddModelError("captcha", "Captcha validation failed");
                return Page();
            }
            if (!ModelState.IsValid)
            {
                _logger.LogDebug("Model state not valid");
                return Page();
            }

            string path = Path.Combine(_templatesPath);
            string template = "ContactMeTemplate.html";
            string FilePath = Path.Combine(path, template);

            StreamReader str = new StreamReader(FilePath);
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[fromEmail]", Input.Email).Replace("[fromName]", Input.Name).Replace("[contactmessage]", Input.Message);
            
            await _emailService.SendAsync("germistry@germistry.com", Input.Subject, mailText, true);
                       
            // On success go to contact result result which is a thankyou page
            _logger.LogDebug("Email sent");
            return RedirectToPage("ContactResult");
        }
    }
}