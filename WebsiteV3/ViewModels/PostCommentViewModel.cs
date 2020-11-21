using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteV3.ViewModels
{
    public class PostCommentViewModel
    {
        //The PostId for the comment
        public int PostId { get; set; }
        public string PostSlug { get; set; }
        //The MainCommentId will determine whether it is a maincomment or subcomment
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm you acknowledge the Commentary Guidelines")]
        public bool GuidelinesConsent { get; set; }
    }
}
