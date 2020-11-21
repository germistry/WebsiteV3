using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV3.Data.Repository;
using WebsiteV3.Models;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.Models.PostComments;
using WebsiteV3.ViewModels;

namespace WebsiteV3.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "SuperAdmin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository _repo;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public UserManagerController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, 
            IRepository repo)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _repo = repo;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var allUsers = await _userManager.Users.ToListAsync();

            var vm = new List<UserManagerViewModel>();

            foreach (ApplicationUser user in allUsers)
            {
                string userId = user.Id;

                var thisvm = new UserManagerViewModel();
                thisvm.UserId = user.Id;
                thisvm.UserName = user.UserName;
                thisvm.Email = user.Email;
                thisvm.IsBanned = user.IsBanned;
                thisvm.SignUpDate = user.SignUpDate;
                thisvm.UserPortfolioItemMainComments = _repo.GetAllPortfolioItemMainComments(userId);
                thisvm.UserPortfolioItemSubComments = _repo.GetAllPortfolioItemSubComments(userId);
                thisvm.UserPostMainComments = _repo.GetAllPostMainComments(userId);
                thisvm.UserPostSubComments = _repo.GetAllPostSubComments(userId);

                vm.Add(thisvm);
            }
            return View(vm);
        }
        public async Task<IActionResult> ManageUserComments(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;

            var vm = new ManageUserCommentsViewModel()
            {
                UserPostMainComments = _repo.GetAllPostMainComments(userId),
                UserPostSubComments = _repo.GetAllPostSubComments(userId),
                UserPortfolioItemMainComments = _repo.GetAllPortfolioItemMainComments(userId),
                UserPortfolioItemSubComments = _repo.GetAllPortfolioItemSubComments(userId)
            };
            return View(vm);
        }
        //Http get to delete a post main comment using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePostMainComment(int id)
        {
            var maincomment = _repo.GetPostMainComment(id);
            bool hasSubs = maincomment.SubComments.Count() > 0;

            if (hasSubs == true)
            {
                var comment = new PostMainComment
                {
                    Id = maincomment.Id,
                    CreatedDate = maincomment.CreatedDate,
                    Message = "This comment has been removed.",
                    UserId = null,
                    PostId = maincomment.PostId
                };
                _repo.UpdatePostMainComment(comment);
            }
            else
            {
                _repo.DeletePostMainComment(id);
            }
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //Http get to delete a post sub comment using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePostSubComment(int id)
        {
            _repo.DeletePostSubComment(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //Http get to delete a portfolio item main comment using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePortfolioItemMainComment(int id)
        {
            var maincomment = _repo.GetPortfolioItemMainComment(id);
            bool hasSubs = maincomment.SubComments.Count() > 0;

            if (hasSubs == true)
            {
                var comment = new PortfolioItemMainComment
                {
                    Id = maincomment.Id,
                    CreatedDate = maincomment.CreatedDate,
                    Message = "This comment has been removed.",
                    UserId = null,
                    PortfolioItemId = maincomment.PortfolioItemId
                };
                _repo.UpdatePortfolioItemMainComment(comment);
            }
            else
            {
                _repo.DeletePortfolioItemMainComment(id);
            }
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //Http get to delete a portfolio item sub comment using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePortfolioItemSubComment(int id)
        {
            _repo.DeletePortfolioItemSubComment(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //PRODUCTION - Test that a signed Externally logged in user gets signed off or cookie deleted if banned.
        public async Task<IActionResult> BanUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            //To ban a user & lockout.  
            if (user.IsBanned == false)
            {
                user.IsBanned = true;
                user.LockoutEnd = new DateTime(2100, 12, 12, 0, 0, 0);
                await _userManager.UpdateAsync(user);
            }
            //To unban a user
            else
            {
                user.IsBanned = false;
                user.LockoutEnd = null;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }
        //Todo - FUTURE search fields for finding users? particular comments?
    }
}