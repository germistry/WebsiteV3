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
    public class EditPostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "";
        [Required]
        public string Slug { get; set; } = "";
        [Required]
        public string Introduction { get; set; }
        [Required]
        public string Body { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        public string Tags { get; set; } = "";
        public int CurrentCategoryId { get; set; } = 0;
        //the selected value in drop down 
        [Required]
        public int CategoryId { get; set; } = 0;
        //the dropdown list
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public string CurrentImage { get; set; } = "";
        public IFormFile Image { get; set; } = null;
        public bool Featured { get; set; } = false;
    }
}
