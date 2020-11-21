using System.Collections.Generic;
using WebsiteV3.Models;

namespace WebsiteV3.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<PortfolioItem> PortfolioItems { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}
