using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebsiteV3.Data.FileManager;
using WebsiteV3.Data.Repository;
using WebsiteV3.Models;
using WebsiteV3.Models.Comments;
using WebsiteV3.ViewModels;

namespace WebsiteV3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repo;
        private readonly IFileManager _fileManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IRepository repo, IFileManager fileManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _repo = repo;
            _fileManager = fileManager;
            _userManager = userManager;
        }

        //Http method Get - Returns the home page. 
        //Todo - with eventually the 'featured posts', featured portfolio projects
        public IActionResult Index()
        {
            return View();
        }
        //Http method Get - Returns about me page. 
        //Todo - Now page, with eventually some more 'featured stuff from my portfolio projects.
        public IActionResult About()
        {
            return View();
        }
        //Http method Get - Returns contact me page. 
        public IActionResult Contact()
        {
            return View();
        }
        //Http method Post - submitted contact form to generate email sent to me to action.
        [HttpPost]
        public IActionResult Contact(ContactViewModel vm)
        {
            //Todo - implement action for contact me submission
            return RedirectToAction("ContactResult");
        }
        //Http method Get - Returns contact result and whether it was successful or not. 
        public IActionResult ContactResult()
        {
            return View();
        }
        //Http method Get - Returns privacy page. 
        //Todo - Create a privacy policy

        public IActionResult Privacy()
        {
            return View();
        }
        //FUNCTION STYLE
        //Http method Get - Returns a page with a list of posts, or supply category to have filtered by
        //category. 
        public IActionResult Blog(int pageNumber, int category, string searchPosts)
        {
            if (pageNumber < 1)
                return RedirectToAction("Blog", new { pageNumber = 1, category });

            var vm = _repo.GetAllPosts(pageNumber, category, searchPosts);

            return View(vm);
        }
        //FUNCTION STYLE
        //Http method Get - Returns the individual post page for a particular post.  
        public IActionResult Post(int id) =>
            View(_repo.GetPost(id));
        //STATEMENT STYLE
        //public IActionResult Post(int id)
        //{
        //    var post = _repo.GetPost(id);
        //    return View(post);
        //}

        //FUNCTION STYLE
        //HttpGet to return the post image through filestream.
        [HttpGet("/PostImage/{postImage}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult PostImage(string postImage) =>
           new FileStreamResult(_fileManager.PostImageStream(postImage),
               $"postImage/{postImage.Substring(postImage.LastIndexOf('.') + 1)}");

        //STATEMENT STYLE
        //[HttpGet("/PostImage/{postImage}")]
        //public IActionResult PostImage(string postImage)
        //{
        //    var mime = postImage.Substring(postImage.LastIndexOf('.') + 1);
        //    return new FileStreamResult(_fileManager.PostImageStream(postImage), $"postImage/{mime}");
        //}

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

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (vm.PostId == 0)
            {
                ModelState.Remove("PostId");
                if (!ModelState.IsValid)
                    return RedirectToAction("PortfolioItem", new { id = vm.PortfolioItemId });
                else
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var portfolioItem = _repo.GetPortfolioItem(vm.PortfolioItemId);
                    if (vm.MainCommentId == 0)
                    {
                        portfolioItem.MainComments ??= new List<MainComment>();

                        portfolioItem.MainComments.Add(new MainComment
                        {
                            Message = vm.Message,
                            CreatedDate = DateTime.Now,
                            User = user

                        });
                        _repo.UpdatePortfolioItem(portfolioItem);
                    }
                    else
                    {
                        var comment = new SubComment
                        {
                            MainCommentId = vm.MainCommentId,
                            Message = vm.Message,
                            CreatedDate = DateTime.Now,
                            User = user

                        };
                        _repo.AddSubComment(comment);
                    }
                    await _repo.SaveChangesAsync();
                    return RedirectToAction("PortfolioItem", new { id = vm.PortfolioItemId });
                }
            }
            else if (vm.PortfolioItemId == 0)
            {
                ModelState.Remove("PortfolioItemId");
                if (!ModelState.IsValid)
                    return RedirectToAction("Post", new { id = vm.PostId });
                else
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var post = _repo.GetPost(vm.PostId);
                    if (vm.MainCommentId == 0)
                    {
                        post.MainComments ??= new List<MainComment>();

                        post.MainComments.Add(new MainComment
                        {
                            Message = vm.Message,
                            CreatedDate = DateTime.Now,
                            User = user

                        });
                        _repo.UpdatePost(post);
                    }
                    else
                    {
                        var comment = new SubComment
                        {
                            MainCommentId = vm.MainCommentId,
                            Message = vm.Message,
                            CreatedDate = DateTime.Now,
                            User = user

                        };
                        _repo.AddSubComment(comment);
                    }
                    await _repo.SaveChangesAsync();
                    return RedirectToAction("Post", new { id = vm.PostId });
                }
            }

            return Redirect("/Error/500");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
