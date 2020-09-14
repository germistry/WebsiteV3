using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models.PostComments;
using WebsiteV3.Models.PortfolioItemComments;

namespace WebsiteV3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[] ProfileImage { get; set; }
        public DateTime SignUpDate { get; set; } = DateTime.Now;
        public List<PostMainComment> UserPostMainComments { get; set; }
        public List<PostSubComment> UserPostSubComments { get; set; }
        public List<PortfolioItemMainComment> UserPortfolioItemMainComments { get; set; }
        public List<PortfolioItemSubComment> UserPortfolioItemSubComments { get; set; }

    }
}
