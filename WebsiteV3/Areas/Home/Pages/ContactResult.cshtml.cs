using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
