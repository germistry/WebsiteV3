using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.ViewModels
{
    public class AboutAssetViewModel
    {
        public int Id { get; set; }
        public string CurrentAsset { get; set; } = "";
        public IFormFile Asset { get; set; } = null;
        public string Caption { get; set; }
        [Required]
        public int AboutId { get; set; } = 0;
        public string ReturnUrl { get; set; }
    }
}
