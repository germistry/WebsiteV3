using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models.Comments;

namespace WebsiteV3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[] ProfileImage { get; set; }
        public DateTime SignUpDate { get; set; } = DateTime.Now;
        public List<MainComment> UserMainComments { get; set; }
        public List<SubComment> UserSubComments { get; set; }
        
    }
}
