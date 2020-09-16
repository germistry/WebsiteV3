using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
