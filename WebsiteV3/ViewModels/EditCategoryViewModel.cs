using Microsoft.AspNetCore.Http;

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
