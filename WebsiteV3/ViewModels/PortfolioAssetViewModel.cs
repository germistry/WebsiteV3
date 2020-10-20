using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.ViewModels
{
    public class PortfolioAssetViewModel
    {

        public int Id { get; set; }
        public string CurrentAsset { get; set; } = "";
        public IFormFile Asset { get; set; } = null;
        public string Caption { get; set; }
        [Required]
        public int PortfolioItemId { get; set; } = 0;
        public string ReturnUrl { get; set; }

    }
}
