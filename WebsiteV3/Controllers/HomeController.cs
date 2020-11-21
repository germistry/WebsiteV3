using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebsiteV3.Data.FileManager;
using WebsiteV3.Data.Repository;
using WebsiteV3.Models;
using WebsiteV3.Models.PostComments;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.ViewModels;
using NETCore.MailKit.Core;
using Microsoft.Extensions.Configuration;
using WebsiteV3.Helpers;

namespace WebsiteV3.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repo;
        private readonly IFileManager _fileManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly string _templatesPath;

        //For designating image types
        private readonly string[] ImageTypes = new[] { ".png", ".jpg", ".jpeg", ".gif" };
        
        public HomeController(ILogger<HomeController> logger, 
            IRepository repo, 
            IFileManager fileManager, 
            UserManager<ApplicationUser> userManager,
            IEmailService emailService, IConfiguration pathConfig)
        {
            _logger = logger;
            _repo = repo;
            _fileManager = fileManager;
            _userManager = userManager;
            _emailService = emailService;
            _templatesPath = pathConfig["Path:Templates"];
        }

        //todo - FUTURE link for uncle les's help page.
        
        //Http method Get - Returns the home page. 
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
        public IActionResult About()
        {
            return View(_repo.GetAllAbout());
        }
        //HttpGet to return the about asset through filestream.
        [HttpGet("/AboutAsset/{aboutAsset}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult AboutAsset(string aboutAsset) =>
            new FileStreamResult(_fileManager.AboutAssetStream(aboutAsset),
                $"aboutAsset/{aboutAsset.Substring(aboutAsset.LastIndexOf('.') + 1)}");

        //Http method Get - Returns privacy page. 
        public IActionResult Privacy()
        {
            return View();
        }
        //Http method Get - Returns terms of use page. 
        public IActionResult Terms()
        {
            return View();
        }
        //Http method Get - Returns cookie policy page. 
        public IActionResult Cookies()
        {
            return View();
        }
        //Http method Get - Returns commentary guidelines policy page. 
        public IActionResult CommentaryGuidelines()
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
        [HttpGet("/Post/{id}/{slug}")]
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

        //HttpGet to return the post asset through filestream.
        [HttpGet("/PostAsset/{postAsset}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult PostAsset(string postAsset)
        {
            var mime = postAsset.Substring(postAsset.LastIndexOf('.'));

            if (ImageTypes.Contains(mime))
            {
                return new FileStreamResult(_fileManager.PostAssetImageStream(postAsset),
                            $"postAsset/{postAsset.Substring(postAsset.LastIndexOf('.') + 1)}");
            }
            else 
            {
                return new FileStreamResult(_fileManager.PostAssetFileStream(postAsset),
                            $"postAsset/{postAsset.Substring(postAsset.LastIndexOf('.') + 1)}");
            }
        }
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
        [HttpGet("/Portfolio/{id}/{slug}")]
        public IActionResult PortfolioItem(int id)
        {
            return View(_repo.GetPortfolioItem(id));
        }

        //HttpGet to return the portfolio item image through filestream.
        [HttpGet("/PortfolioItemImage/{portfolioItemImage}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult PortfolioItemImage(string portfolioItemImage) =>
            new FileStreamResult(_fileManager.PortfolioItemImageStream(portfolioItemImage),
            $"portfolioItemImage/{portfolioItemImage.Substring(portfolioItemImage.LastIndexOf('.') + 1)}");

        //HttpGet to return the portfolio asset image through filestream.
        [HttpGet("/PortfolioAsset/{portfolioAsset}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult PortfolioAsset(string portfolioAsset)
        {
            var mime = portfolioAsset.Substring(portfolioAsset.LastIndexOf('.'));

            if (ImageTypes.Contains(mime))
            {
                return new FileStreamResult(_fileManager.PortfolioAssetStream(portfolioAsset),
                $"portfolioAsset/{portfolioAsset.Substring(portfolioAsset.LastIndexOf('.') + 1)}");
            }
            else
            {
                return new FileStreamResult(_fileManager.PortfolioAssetFileStream(portfolioAsset),
                            $"portfolioAsset/{portfolioAsset.Substring(portfolioAsset.LastIndexOf('.') + 1)}");
            }
        }

        //HttpPost to add the new main or subcomment and return the post page.
        [HttpPost]
        public async Task<IActionResult> PostComment(PostCommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.PostId, slug = vm.PostSlug });
            else
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (vm.MainCommentId == 0)
                {
                    var comment = new PostMainComment
                    {
                        PostId = vm.PostId,
                        PostSlug = vm.PostSlug,
                        Message = vm.Message,
                        CreatedDate = DateTime.Now,
                        User = user
                    };
                    _repo.AddPostMainComment(comment);
                    string notifyMailText = EmailHelper.BuildTemplate(_templatesPath, "NewCommentTemplate.html");
                    notifyMailText = notifyMailText.Replace("[username]", user.UserName).Replace("[slug]", vm.PostSlug).Replace("[message]", vm.Message);
                    await _emailService.SendAsync("germistry@germistry.com", "New Post Main Comment Added", notifyMailText, true);
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
                    string notifyMailText = EmailHelper.BuildTemplate(_templatesPath, "NewCommentTemplate.html");
                    notifyMailText = notifyMailText.Replace("[username]", user.UserName).Replace("[slug]", vm.MainCommentId.ToString()).Replace("[message]", vm.Message);
                    await _emailService.SendAsync("germistry@germistry.com", "New Post Sub Comment Added", notifyMailText, true);
                }
                await _repo.SaveChangesAsync();
                     
                return RedirectToAction("Post", new { id = vm.PostId, slug = vm.PostSlug });
            }
        }
        //HttpPost to add the new main or subcomment and return the portfolio item page.
        [HttpPost]
        public async Task<IActionResult> PortfolioItemComment(PortfolioItemCommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("PortfolioItem", new { id = vm.PortfolioItemId, slug = vm.PortfolioItemSlug });
            else
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (vm.MainCommentId == 0)
                {
                    var comment = new PortfolioItemMainComment
                    {
                        PortfolioItemId = vm.PortfolioItemId,
                        PortfolioItemSlug = vm.PortfolioItemSlug,
                        Message = vm.Message,
                        CreatedDate = DateTime.Now,
                        User = user
                    };
                    _repo.AddPortfolioItemMainComment(comment);
                    string notifyMailText = EmailHelper.BuildTemplate(_templatesPath, "NewCommentTemplate.html");
                    notifyMailText = notifyMailText.Replace("[username]", user.UserName).Replace("[slug]", vm.PortfolioItemSlug).Replace("[message]", vm.Message);
                    await _emailService.SendAsync("germistry@germistry.com", "New Portfolio Main Comment Added", notifyMailText, true);
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
                    string notifyMailText = EmailHelper.BuildTemplate(_templatesPath, "NewCommentTemplate.html");
                    notifyMailText = notifyMailText.Replace("[username]", user.UserName).Replace("[slug]", vm.MainCommentId.ToString()).Replace("[message]", vm.Message);
                    await _emailService.SendAsync("germistry@germistry.com", "New Portfolio Sub Comment Added", notifyMailText, true);
                }
                await _repo.SaveChangesAsync();
                return RedirectToAction("PortfolioItem", new { id = vm.PortfolioItemId, slug = vm.PortfolioItemSlug });
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
