using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Data.FileManager;
using WebsiteV3.Data.Repository;
using WebsiteV3.Models;
using WebsiteV3.ViewModels;

namespace WebsiteV3.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class PanelController : Controller
    {
        private readonly ILogger<PanelController> _logger;
        private readonly IRepository _repo;
        private readonly IFileManager _fileManager;

        public PanelController(ILogger<PanelController> logger, IRepository repo, IFileManager fileManager)
        {
            _logger = logger;
            _repo = repo;
            _fileManager = fileManager;
        }
        //HttpGet for admin panel index page with links to all different management stuff. 
        public IActionResult Index()
        {
            return View();
        }
        //HttpGet for admin panel portfolio page, shows all portfolioItems. 
        public IActionResult Posts()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }
        //HttpGet for post edit page for the selected post. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a new post.
        [HttpGet]
        public IActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return View(new EditPostViewModel());
            }
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new EditPostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    CurrentImage = post.Image,
                    Description = post.Description,
                    Tags = post.Tags
                    //,
                    //Category = post.Category
                });

            }

        }
        //HttpPost task that actually does the updating and saving of new posts, and redirects the page
        //back to index even if it's taking some time for the changes to be saved to the database.
        [HttpPost]
        public async Task<IActionResult> EditPost(EditPostViewModel postvm)
        {
            var post = new Post
            {
                Id = postvm.Id,
                Title = postvm.Title,
                Body = postvm.Body,
                Description = postvm.Description,
                Tags = postvm.Tags
                //,
                //Category = postvm.Category
            };
            if (postvm.Image == null)
                post.Image = postvm.CurrentImage;
            else
            {
                if (!string.IsNullOrEmpty(postvm.CurrentImage))
                    _fileManager.RemovePostImage(postvm.CurrentImage);
                post.Image = _fileManager.SavePostImage(postvm.Image);
            }
            if (post.Id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);

            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }
            else
                return View(post);
        }
        //Http get to delete a particular post using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePost(int id)
        {
            _repo.DeletePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        //HttpGet for admin panel portfolio page, shows all portfolioItems. 
        public IActionResult PortfolioItems()
        {
            var portfolioItems = _repo.GetAllPortfolioItems();
            return View(portfolioItems);
        }
        //HttpGet for portfolio edit page for the selected item. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a new portfolio item.
        [HttpGet]
        public IActionResult EditPortfolioItem(int? id)
        {
            if (id == null)
            {
                return View(new EditPortfolioItemViewModel());
            }
            else
            {
                var portfolioItem = _repo.GetPortfolioItem((int)id);
                return View(new EditPortfolioItemViewModel
                {
                    Id = portfolioItem.Id,
                    Title = portfolioItem.Title,
                    Body = portfolioItem.Body,
                    CurrentImage = portfolioItem.Image,
                    Description = portfolioItem.Description,
                    Tags = portfolioItem.Tags
                    //,
                    //Category = portfolioItem.Category
                });
            }

        }
        //HttpPost task that actually does the updating and saving of new portfolio items, and redirects the page
        //back to portfolioitems even if it's taking some time for the changes to be saved to the database.
        [HttpPost]
        public async Task<IActionResult> EditPortfolioItem(EditPortfolioItemViewModel portfoliovm)
        {
            var portfolioItem = new PortfolioItem
            {
                Id = portfoliovm.Id,
                Title = portfoliovm.Title,
                Body = portfoliovm.Body,
                Description = portfoliovm.Description,
                Tags = portfoliovm.Tags
                //,
                //Category = portfoliovm.Category
            };

            if (portfoliovm.Image == null)
                portfolioItem.Image = portfoliovm.CurrentImage;
            else
            {
                if (!string.IsNullOrEmpty(portfoliovm.CurrentImage))
                    _fileManager.RemovePortfolioImage(portfoliovm.CurrentImage);
                portfolioItem.Image = _fileManager.SavePortfolioItemImage(portfoliovm.Image);
            }
            if (portfolioItem.Id > 0)
                _repo.UpdatePortfolioItem(portfolioItem);
            else
                _repo.AddPortfolioItem(portfolioItem);

            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("PortfolioItems");
            }
            else
                return View(portfolioItem);
        }
        //Http get to delete a particular portfolio item using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePortfolioItem(int id)
        {
            _repo.DeletePortfolioItem(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("PortfolioItems");

        }
    }
}
