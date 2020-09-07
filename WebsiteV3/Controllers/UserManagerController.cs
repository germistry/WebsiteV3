using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models;
using WebsiteV3.ViewModels;

namespace WebsiteV3.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManagerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            
            var vm = new List<UserManagerViewModel>();
            
            foreach (ApplicationUser user in allUsers)
            {
                var thisvm = new UserManagerViewModel();
                thisvm.UserId = user.Id;
                thisvm.UserName = user.UserName;
                thisvm.Email = user.Email;
                //Todo - add comments for users                 
                vm.Add(thisvm);
            }
            return View(vm);
        }
        //Todo - Need some kind of "BanUser" action for naughty users, using the email address. 
        //Todo - search fields for finding users
    }
}
