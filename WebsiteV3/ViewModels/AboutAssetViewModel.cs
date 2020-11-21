using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
