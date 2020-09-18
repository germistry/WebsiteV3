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
        [PersonalData]
        public byte[] ProfileImage { get; set; }
        public DateTime SignUpDate { get; set; } = DateTime.Now;
        [PersonalData]
        public List<PostMainComment> UserPostMainComments { get; set; }
        [PersonalData]
        public List<PostSubComment> UserPostSubComments { get; set; }
        [PersonalData]
        public List<PortfolioItemMainComment> UserPortfolioItemMainComments { get; set; }
        [PersonalData]
        public List<PortfolioItemSubComment> UserPortfolioItemSubComments { get; set; }

    }
}
