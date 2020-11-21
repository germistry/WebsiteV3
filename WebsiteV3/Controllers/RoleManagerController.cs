using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV3.Models;
using WebsiteV3.ViewModels;

namespace WebsiteV3.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleManagerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var allRoles = await _roleManager.Roles.ToListAsync();
            
            var vm = new List<RoleManagerViewModel>();
            foreach (IdentityRole r in allRoles)
            {
                var thisvm = new RoleManagerViewModel();
                thisvm.RoleId = r.Id;
                thisvm.RoleName = r.Name;
                var usersInRole = await _userManager.GetUsersInRoleAsync(r.Name);
                thisvm.NumberOfUsers = usersInRole.Count();
                vm.Add(thisvm);
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveRole(string roleName)
        {
            if (roleName != null)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
    }
}
