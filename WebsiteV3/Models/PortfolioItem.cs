using System;
using System.Collections.Generic;
using WebsiteV3.Models.PortfolioItemComments;

namespace WebsiteV3.Models
{
    public class PortfolioItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Introduction { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<PortfolioItemMainComment> MainComments { get; set; }
        public string SourceCodeLink { get; set; }
        public bool CommentsAllowed { get; set; } = true;
        public List<PortfolioAsset> PortfolioAssets { get; set; }
    }
}
