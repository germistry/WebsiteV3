using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Models
{
    public class PortfolioAsset
    {
        public int Id { get; set; }
        public string Asset { get; set; }
        public PortfolioItem PortfolioItem { get; set; }
        public string Caption { get; set; }
    }
}
