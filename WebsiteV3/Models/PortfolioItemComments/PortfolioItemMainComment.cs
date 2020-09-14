using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Models.PortfolioItemComments
{
    public class PortfolioItemMainComment : PortfolioItemComment
    {
        public List<PortfolioItemSubComment> SubComments { get; set; }
        public int PortfolioItemId { get; set; }
    }
}
