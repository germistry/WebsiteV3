using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.ViewModels
{
    public class EditPortfolioItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Introduction { get; set; }
        public string Body { get; set; } = "";
        public string Description { get; set; } = "";
        public string Tags { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public string CurrentImage { get; set; } = "";
        public IFormFile Image { get; set; } = null;
        public string SourceCodeLink { get; set; }
    }
}
