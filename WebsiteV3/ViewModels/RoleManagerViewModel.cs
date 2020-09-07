using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
