using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models;

namespace WebsiteV3.ViewModels
{
    public class PortfolioItemCommentViewModel
    {       
        //The PortfolioItemId for the comment
        public int PortfolioItemId { get; set; }
        //The MainCommentId will determine whether it is a maincomment or subcomment
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
