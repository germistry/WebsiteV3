using System.Collections.Generic;

namespace WebsiteV3.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public List<Post> Posts { get; set; }
        public List<PortfolioItem> PortfolioItems { get; set; }
    }
}
