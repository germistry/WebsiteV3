using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var categories = _repo.GetAllCategories();
            return View(categories);
        }
        //HttpGet for admin panel post page, shows all posts. 
        public IActionResult ManageBlog()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }
        //HttpGet for post edit page for the selected post. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a new post.
        [HttpGet]
        public IActionResult EditPost(int? id)
        {
            var categoryList = _repo.GetAllCategories().ToList();
            var dropDownList = new SelectList(categoryList.Select(item => new SelectListItem
            {
                Text = item.CategoryName,
                Value = item.Id.ToString()
            }).ToList(), "Value", "Text");

            if (id == null)
            {
                var vm = new EditPostViewModel()
                {
                    CategoryList = dropDownList
                };
                return View(vm);
            }
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new EditPostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Introduction = post.Introduction,
                    Body = post.Body,
                    CurrentImage = post.Image,
                    Description = post.Description,
                    Tags = post.Tags,
                    CurrentCategoryId = post.Category.Id,
                    CategoryId = post.Category.Id,
                    CategoryList = dropDownList
                }); ;
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
                Introduction = postvm.Introduction,
                Body = postvm.Body,
                Description = postvm.Description,
                Tags = postvm.Tags
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
            {
                if (postvm.CategoryId != postvm.CurrentCategoryId)
                {
                    var category = _repo.GetCategoryNoTracking(postvm.CategoryId);
                    post.Category = category;
                }
                _repo.UpdatePost(post);
            }
            else
            {
                var category = _repo.GetCategory(postvm.CategoryId);
                post.Category = category;
                _repo.AddPost(post);
            }
            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("ManageBlog");
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
            return RedirectToAction("ManageBlog");
        }
        //HttpGet for admin panel portfolio page, shows all portfolioItems. 
        public IActionResult ManagePortfolio()
        {
            var portfolioItems = _repo.GetAllPortfolioItems();
            return View(portfolioItems);
        }
        //HttpGet for portfolio edit page for the selected item. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a new portfolio item.
        [HttpGet]
        public IActionResult EditPortfolioItem(int? id)
        {
            var categoryList = _repo.GetAllCategories().ToList();
            var dropDownList = new SelectList(categoryList.Select(item => new SelectListItem
            {
                Text = item.CategoryName,
                Value = item.Id.ToString()
            }).ToList(), "Value", "Text");

            if (id == null)
            {
                var vm = new EditPortfolioItemViewModel()
                {
                    CategoryList = dropDownList
                };
                return View(vm);
            }
            else
            {
                var portfolioItem = _repo.GetPortfolioItem((int)id);
                return View(new EditPortfolioItemViewModel
                {
                    Id = portfolioItem.Id,
                    Title = portfolioItem.Title,
                    Introduction = portfolioItem.Introduction,
                    Body = portfolioItem.Body,
                    CurrentImage = portfolioItem.Image,
                    Description = portfolioItem.Description,
                    Tags = portfolioItem.Tags,
                    CurrentCategoryId = portfolioItem.Category.Id,
                    CategoryId = portfolioItem.Category.Id,
                    CategoryList = dropDownList,
                    SourceCodeLink = portfolioItem.SourceCodeLink
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
                Introduction = portfoliovm.Introduction,
                Body = portfoliovm.Body,
                Description = portfoliovm.Description,
                Tags = portfoliovm.Tags, 
                SourceCodeLink = portfoliovm.SourceCodeLink                
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
            {
                if (portfoliovm.CategoryId != portfoliovm.CurrentCategoryId)
                {
                    var category = _repo.GetCategoryNoTracking(portfoliovm.CategoryId);
                    portfolioItem.Category = category;
                }
                _repo.UpdatePortfolioItem(portfolioItem);
            }
            else
            {
                var category = _repo.GetCategory(portfoliovm.CategoryId);
                portfolioItem.Category = category;
                _repo.AddPortfolioItem(portfolioItem);
            }
            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("ManagePortfolio");
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
            return RedirectToAction("ManagePortfolio");
        }
        //HttpGet for category edit page for the selected category. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a new category.
        //Todo - add picture for category, number of posts/portfolio items linked to category
        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return View(new EditCategoryViewModel());
            }
            else
            {
                var category = _repo.GetCategory((int)id);
                return View(new EditCategoryViewModel
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                });
            }
        }
        //HttpPost task that actually does the updating and saving of new categories, and redirects the page
        //back to index even if it's taking some time for the changes to be saved to the database.
        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel categoryvm)
        {
            var category = new Category
            {
                Id = categoryvm.Id,
                CategoryName = categoryvm.CategoryName
            };
            
            if (category.Id > 0)
                _repo.UpdateCategory(category);
            else
                _repo.AddCategory(category);

            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }
            else
                return View(category);
        }
        //Http get to delete a particular category using its id. 
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _repo.DeleteCategory(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
