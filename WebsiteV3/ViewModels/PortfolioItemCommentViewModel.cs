using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteV3.ViewModels
{
    public class PortfolioItemCommentViewModel
    {
        //The PortfolioItemId for the comment
        public int PortfolioItemId { get; set; }
        public string PortfolioItemSlug { get; set; }
        //The MainCommentId will determine whether it is a maincomment or subcomment
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        [StringLength(12000, ErrorMessage = "The message must be at least 5 characters long.", MinimumLength = 5)]
        public string Message { get; set; }
        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm you acknowledge the Commentary Guidelines")]
        public bool GuidelinesConsent { get; set; }
    }
}
