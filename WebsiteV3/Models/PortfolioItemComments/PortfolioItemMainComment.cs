using System.Collections.Generic;

namespace WebsiteV3.Models.PortfolioItemComments
{
    public class PortfolioItemMainComment : PortfolioItemComment
    {
        public List<PortfolioItemSubComment> SubComments { get; set; }
        public int PortfolioItemId { get; set; }
        public string PortfolioItemSlug { get; set; }
    }
}
