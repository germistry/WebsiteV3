using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Data.Repository;
using WebsiteV3.Models;

namespace WebsiteV3.Areas.Identity.Pages.Account.Manage
{
    public class DownloadUserCommentsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DownloadPersonalDataModel> _logger;
        private readonly IRepository _repo;

        public DownloadUserCommentsModel(UserManager<ApplicationUser> userManager, 
            IRepository repo,
            ILogger<DownloadPersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
            _repo = repo;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their user comments.", _userManager.GetUserId(User));


            var model = new ApplicationUser()
            {
                UserPostMainComments = _repo.GetAllPostMainComments(user.Id),
                UserPostSubComments = _repo.GetAllPostSubComments(user.Id),
                UserPortfolioItemMainComments = _repo.GetAllPortfolioItemMainComments(user.Id),
                UserPortfolioItemSubComments = _repo.GetAllPortfolioItemSubComments(user.Id)
            };
            int postMainCommentCount = model.UserPostMainComments.Count();
            int postSubCommentCount = model.UserPostSubComments.Count();
            int portfolioItemMainCommentCount = model.UserPortfolioItemMainComments.Count();
            int portfolioItemSubCommentCount = model.UserPortfolioItemSubComments.Count();
            
            var builder = new StringBuilder();

            builder.AppendLine("PostComments");
            builder.AppendLine("Id,Message,CreatedDate");
            if (postMainCommentCount == 0)
                builder.AppendLine("No post comments have been made by user.");
            else
            {
                foreach (var postMaincomment in model.UserPostMainComments)
                {
                    builder.AppendLine($"{postMaincomment.Id},{postMaincomment.Message},{postMaincomment.CreatedDate}");
                };
            }

            builder.AppendLine("PostReplies");
            builder.AppendLine("Id,Message,CreatedDate");
            if (postSubCommentCount == 0)
                builder.AppendLine("No post replies have been made by user.");
            else
            {
                foreach (var postSubcomment in model.UserPostSubComments)
                {
                    builder.AppendLine($"{postSubcomment.Id},{postSubcomment.Message},{postSubcomment.CreatedDate}");
                };
            }

            builder.AppendLine("PortfolioItemComments");
            builder.AppendLine("Id,Message,CreatedDate");
            if (portfolioItemMainCommentCount == 0)
                builder.AppendLine("No portfolio item comments have been made by user.");
            else
            {
                foreach (var portfolioItemMaincomment in model.UserPortfolioItemMainComments)
                {
                    builder.AppendLine($"{portfolioItemMaincomment.Id},{portfolioItemMaincomment.Message},{portfolioItemMaincomment.CreatedDate}");
                };
            }

            builder.AppendLine("PortfolioItemReplies");
            builder.AppendLine("Id,Message,CreatedDate");
            if (portfolioItemSubCommentCount == 0)
                builder.AppendLine("No portfolio item replies have been made by user.");
            else
            {
                foreach (var portfolioItemSubcomment in model.UserPortfolioItemSubComments)
                {
                    builder.AppendLine($"{portfolioItemSubcomment.Id},{portfolioItemSubcomment.Message},{portfolioItemSubcomment.CreatedDate}");
                }; 
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "userComments.csv");
        }
    }
}
