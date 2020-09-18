using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.Models.PostComments;

namespace WebsiteV3.ViewModels
{
    public class ManageUserCommentsViewModel
    {
        public IEnumerable<PostMainComment> UserPostMainComments { get; set; }
        public IEnumerable<PostSubComment> UserPostSubComments { get; set; }
        public IEnumerable<PortfolioItemMainComment> UserPortfolioItemMainComments { get; set; }
        public IEnumerable<PortfolioItemSubComment> UserPortfolioItemSubComments { get; set; }
    }
}
