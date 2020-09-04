using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models.Comments;

namespace WebsiteV3.Models
{
    public class UserOptions : IdentityUser
    {
        public string DisplayName { get; set; }
        public string AvatarImage { get; set; }
        public DateTime SignUpDate { get; set; } = DateTime.Now;
        public List<MainComment> UserMainComments { get; set; }
        public List<SubComment> UserSubComments { get; set; }
        
    }
}
