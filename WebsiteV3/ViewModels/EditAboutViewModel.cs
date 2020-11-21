using System.ComponentModel.DataAnnotations;

namespace WebsiteV3.ViewModels
{
    public class EditAboutViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Heading { get; set; } = "";
        public int PageOrder { get; set; }
        [Required]
        public string Body { get; set; } = "";
    }
}
