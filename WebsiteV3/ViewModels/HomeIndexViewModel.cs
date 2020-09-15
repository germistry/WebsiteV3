using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
