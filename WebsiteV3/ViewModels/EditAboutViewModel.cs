using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
