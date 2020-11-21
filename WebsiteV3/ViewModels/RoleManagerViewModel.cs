using System.Collections.Generic;
using WebsiteV3.Models;

namespace WebsiteV3.ViewModels
{
    public class RoleManagerViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int NumberOfUsers { get; set; }
        public List<ApplicationUser> Users { get; set; }
      
    }
}
