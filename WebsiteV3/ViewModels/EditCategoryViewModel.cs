using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.ViewModels
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CurrentImage { get; set; } = "";
        public IFormFile Image { get; set; } = null;
    }
}
