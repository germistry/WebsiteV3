using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models.Comments;

namespace WebsiteV3.ViewModels
{
    public class UserManagerViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<MainComment> UserMainComments { get; set; }
        public List<SubComment> UserSubComments { get; set; }
    }
}
