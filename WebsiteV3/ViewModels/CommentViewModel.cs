using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.ViewModels
{
    public class CommentViewModel
    {
        //The PostId for the comment OR
        [Required]
        public int PostId { get; set; }
        //The PortfolioItemId for the comment
        [Required]
        public int PortfolioItemId { get; set; }
        //The MainCommentId will determine whether it is a maincomment or subcomment
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
