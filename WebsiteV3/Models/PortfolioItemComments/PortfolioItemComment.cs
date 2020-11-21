using System;

namespace WebsiteV3.Models.PortfolioItemComments
{
    public class PortfolioItemComment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
