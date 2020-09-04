using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models;

namespace WebsiteV3.ViewModels
{
    public class PortfolioViewModel
    {
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public int CategoryId { get; set; }
        public string SearchItems { get; set; }
        public IEnumerable<PortfolioItem> PortfolioItems { get; set; }
        public IEnumerable<int> Pages { get; internal set; }
    }
}
