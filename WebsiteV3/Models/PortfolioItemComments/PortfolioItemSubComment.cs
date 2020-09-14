using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Models.PortfolioItemComments
{
    public class PortfolioItemSubComment : PortfolioItemComment
    {
        public int PortfolioItemMainCommentId { get; set; }
    }
}
