using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebsiteV3.Data.Repository;
using WebsiteV3.Models;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.Models.PostComments;

namespace WebsiteV3.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRepository _repo;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IRepository repo,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repo = repo;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }
            //For deleting user comments
            var model = new ApplicationUser()
            {
                UserPostMainComments = _repo.GetAllPostMainComments(user.Id),
                UserPostSubComments = _repo.GetAllPostSubComments(user.Id),
                UserPortfolioItemMainComments = _repo.GetAllPortfolioItemMainComments(user.Id),
                UserPortfolioItemSubComments = _repo.GetAllPortfolioItemSubComments(user.Id)
            };
            foreach (var comment in model.UserPostSubComments)
            {
                _repo.DeletePostSubComment(comment.Id);
                await _repo.SaveChangesAsync();
            };
            foreach (var comment in model.UserPostMainComments)
            {
                var maincomment = _repo.GetPostMainComment(comment.Id);
                bool hasSubs = maincomment.SubComments.Count() > 0;

                if (hasSubs == true)
                {
                    var mc = new PostMainComment
                    {
                        Id = maincomment.Id,
                        CreatedDate = maincomment.CreatedDate,
                        Message = "This comment has been removed.",
                        UserId = null,
                        PostId = maincomment.PostId
                    };
                    _repo.UpdatePostMainComment(mc);
                }
                else
                {
                    _repo.DeletePostMainComment(comment.Id);
                }
                await _repo.SaveChangesAsync();
            };
            foreach (var comment in model.UserPortfolioItemSubComments)
            {
                _repo.DeletePortfolioItemSubComment(comment.Id);
                await _repo.SaveChangesAsync();
            };
            foreach (var comment in model.UserPortfolioItemMainComments)
            {
                var maincomment = _repo.GetPortfolioItemMainComment(comment.Id);
                bool hasSubs = maincomment.SubComments.Count() > 0;

                if (hasSubs == true)
                {
                    var mc = new PortfolioItemMainComment
                    {
                        Id = maincomment.Id,
                        CreatedDate = maincomment.CreatedDate,
                        Message = "This comment has been removed.",
                        UserId = null,
                        PortfolioItemId = maincomment.PortfolioItemId
                    };
                    _repo.UpdatePortfolioItemMainComment(mc);
                }
                else
                {
                    _repo.DeletePortfolioItemMainComment(comment.Id);
                }
                await _repo.SaveChangesAsync();
            };
            
            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
        
    }
}
