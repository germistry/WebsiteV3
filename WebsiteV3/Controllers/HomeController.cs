using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WebsiteV3.Data.FileManager;
using WebsiteV3.Data.Repository;
using WebsiteV3.Models;
using WebsiteV3.Models.PostComments;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.ViewModels;
using WebsiteV3.Services;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace WebsiteV3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repo;
        private readonly IFileManager _fileManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, 
            IRepository repo, 
            IFileManager fileManager, 
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _logger = logger;
            _repo = repo;
            _fileManager = fileManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        //Http method Get - Returns the home page. 
        //todo - link for uncle les's help page.
        public IActionResult Index()
        {    
            return View(_repo.GetFeatures());
        }
        //HttpGet to return the category image through filestream.
        [HttpGet("/CategoryImage/{categoryImage}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult CategoryImage(string categoryImage) =>
           new FileStreamResult(_fileManager.CategoryImageStream(categoryImage),
               $"categoryImage/{categoryImage.Substring(categoryImage.LastIndexOf('.') + 1)}");

        //Http method Get - Returns about me page. 
        //Todo - Make this about page editable by the admin.
        public IActionResult About()
        {
            return View();
        }
        //Http method Get - Returns contact me page. 
        public IActionResult Contact()
        {
            return RedirectToPage("ContactMe");
        }
        //////Http method Post - submitted contact form to generate email sent to me to action.
        ////[HttpPost]
        ////public async Task<IActionResult> Contact(ContactViewModel model)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        var vm = new ContactViewModel()
        ////        {
        ////            Email = model.Email,
        ////            Subject = model.Subject,
        ////            Message = model.Message

        ////        };
                
        ////        await _emailSender.SendEmailAsync(vm.Email, vm.Subject, vm.Message);
          
        ////        _logger.LogInformation("A Contact Email has been sent.");
          
        ////    }
        ////    return RedirectToAction("ContactResult");
        ////}

        
        //Http method Get - Returns privacy page. 
        //Todo - Create a privacy policy

        public IActionResult Privacy()
        {
            return View();
        }
        //Http method Get - Returns a page with a list of posts, or supply category to have filtered by
        //category. 
        public IActionResult Blog(int pageNumber, int category, string searchPosts)
        {
            if (pageNumber < 1)
                return RedirectToAction("Blog", new { pageNumber = 1, category });

            var vm = _repo.GetAllPosts(pageNumber, category, searchPosts);

            return View(vm);
        }
        //Http method Get - Returns the individual post page for a particular post.  
        [HttpGet]
        public IActionResult Post(int id)
        {
            return View(_repo.GetPost(id));
        }

        //HttpGet to return the post image through filestream.
        [HttpGet("/PostImage/{postImage}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult PostImage(string postImage) =>
           new FileStreamResult(_fileManager.PostImageStream(postImage),
               $"postImage/{postImage.Substring(postImage.LastIndexOf('.') + 1)}");

        //Http method Get - Returns a page with a list of portfolio items, or supply category to have 
        //filtered by category. 
        public IActionResult Portfolio(int pageNumber, int category, string searchItems)
        {
            if (pageNumber < 1)
                return RedirectToAction("Portfolio", new { pageNumber = 1, category });

            var vm = _repo.GetAllPortfolioItems(pageNumber, category, searchItems);

            return View(vm);
        }
        //Http method Get - Returns the individual portfolio page for a particular portfolio item.  
        public IActionResult PortfolioItem(int id) =>
            View(_repo.GetPortfolioItem(id));

        //HttpGet to return the portfolio item image through filestream.
        [HttpGet("/PortfolioItemImage/{portfolioItemImage}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult PortfolioItemImage(string portfolioItemImage) =>
            new FileStreamResult(_fileManager.PortfolioItemImageStream(portfolioItemImage),
            $"portfolioItemImage/{portfolioItemImage.Substring(portfolioItemImage.LastIndexOf('.') + 1)}");
        //HttpPost to add the new main or subcomment and return the post page.
        [HttpPost]
        public async Task<IActionResult> PostComment(PostCommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.PostId });
            else
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (vm.MainCommentId == 0)
                {
                    var comment = new PostMainComment
                    {
                        PostId = vm.PostId,
                        Message = vm.Message,
                        CreatedDate = DateTime.Now,
                        User = user
                    };
                    _repo.AddPostMainComment(comment);
                }
                else
                {
                    var comment = new PostSubComment
                    {
                        PostMainCommentId = vm.MainCommentId,
                        Message = vm.Message,
                        CreatedDate = DateTime.Now,
                        User = user
                    };
                    _repo.AddPostSubComment(comment);
                }
                await _repo.SaveChangesAsync();
                return RedirectToAction("Post", new { id = vm.PostId });
            }
        }
        //HttpPost to add the new main or subcomment and return the portfolio item page.
        [HttpPost]
        public async Task<IActionResult> PortfolioItemComment(PortfolioItemCommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("PortfolioItem", new { id = vm.PortfolioItemId });
            else
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (vm.MainCommentId == 0)
                {
                    var comment = new PortfolioItemMainComment
                    {
                        PortfolioItemId = vm.PortfolioItemId,
                        Message = vm.Message,
                        CreatedDate = DateTime.Now,
                        User = user
                    };
                    _repo.AddPortfolioItemMainComment(comment);
                }
                else
                {
                    var comment = new PortfolioItemSubComment
                    {
                        PortfolioItemMainCommentId = vm.MainCommentId,
                        Message = vm.Message,
                        CreatedDate = DateTime.Now,
                        User = user
                    };
                    _repo.AddPortfolioItemSubComment(comment);
                }
                await _repo.SaveChangesAsync();
                return RedirectToAction("PortfolioItem", new { id = vm.PortfolioItemId });
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
