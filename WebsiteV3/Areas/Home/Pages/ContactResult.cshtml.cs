using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebsiteV3.Areas.Home.Pages
{
    [AllowAnonymous]
    public class ContactResultModel :PageModel
    {
        public void OnGet()
        {

        }
    }
}
