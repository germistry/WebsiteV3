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
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.Models.PostComments;
using WebsiteV3.ViewModels;
using WebsiteV3.Helpers;

namespace WebsiteV3.Controllers
{
    [AutoValidateAntiforgeryToken]
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
                    Slug = post.Slug,
                    CreatedDate = post.CreatedDate,
                    Introduction = post.Introduction,
                    Body = post.Body,
                    CurrentImage = post.Image,
                    Description = post.Description,
                    Tags = post.Tags,
                    CurrentCategoryId = post.Category.Id,
                    CategoryId = post.Category.Id,
                    CategoryList = dropDownList,
                    Featured = post.Featured,
                    CommentsAllowed = post.CommentsAllowed
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
                Tags = postvm.Tags,
                Featured = postvm.Featured,
                CommentsAllowed = postvm.CommentsAllowed,
                Slug = SlugGenerator.ToSlug(postvm.Title)
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
                post.CreatedDate = postvm.CreatedDate;
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
                    Slug = portfolioItem.Slug,
                    CreatedDate = portfolioItem.CreatedDate,
                    Introduction = portfolioItem.Introduction,
                    Body = portfolioItem.Body,
                    CurrentImage = portfolioItem.Image,
                    Description = portfolioItem.Description,
                    Tags = portfolioItem.Tags,
                    CurrentCategoryId = portfolioItem.Category.Id,
                    CategoryId = portfolioItem.Category.Id,
                    CategoryList = dropDownList,
                    SourceCodeLink = portfolioItem.SourceCodeLink,
                    CommentsAllowed = portfolioItem.CommentsAllowed
                }); ;
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
                CommentsAllowed = portfoliovm.CommentsAllowed,
                SourceCodeLink = portfoliovm.SourceCodeLink,
                Slug = SlugGenerator.ToSlug(portfoliovm.Title)
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
                portfolioItem.CreatedDate = portfoliovm.CreatedDate;
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
                    CategoryName = category.CategoryName,
                    CurrentImage = category.Image
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
            if (categoryvm.Image == null)
                category.Image = categoryvm.CurrentImage;
            else
            {
                if (!string.IsNullOrEmpty(categoryvm.CurrentImage))
                    _fileManager.RemoveCategoryImage(categoryvm.CurrentImage);
                category.Image = _fileManager.SaveCategoryImage(categoryvm.Image);
            }
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
        //Http Get for Manage Blog Comments.
        [HttpGet]
        public IActionResult ManageBlogComments(int id)
        {
            return View(_repo.GetPost(id));
        }

        //Http get to delete/or replace a post main comment using its id. 
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
            return RedirectToAction("ManageBlog");
        }
        //Http get to delete a post sub comment using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePostSubComment(int id)
        {
            _repo.DeletePostSubComment(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("ManageBlog");
        }
        //Http Get for Managing Portfolio Comments.
        [HttpGet]
        public IActionResult ManagePortfolioComments(int id)
        {
            return View(_repo.GetPortfolioItem(id));
        }
        //Http get to delete/or replace a portfolio item main comment using its id. 
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
            return RedirectToAction("ManagePortfolio");
        }
        //Http get to delete a portfolio item sub comment using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePortfolioItemSubComment(int id)
        {
            _repo.DeletePortfolioItemSubComment(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("ManagePortfolio");
        }
        //HttpGet for admin panel post assets page, shows all post assets.
        public IActionResult ManageBlogAssets()
        {
            var postAssets = _repo.GetAllPostAssets();
            return View(postAssets);
        }
        //Http Get for Managing Post Assets.
        [HttpGet]
        public IActionResult ManageAssetsForPost(int id)
        {
            return View(_repo.GetPost(id));
        }
        //HttpGet for post asset edit page for the selected post. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a new post.
        [HttpGet]
        public IActionResult EditPostAsset(int? id, int postid, string returnurl)
        {
            if (id == null)
            {
                var vm = new PostAssetViewModel()
                {
                    PostId = postid,
                    ReturnUrl = returnurl

                };
                return View(vm);
            }
            else
            {
                var postAsset = _repo.GetPostAsset((int)id);
                return View(new PostAssetViewModel
                {
                    Id = postAsset.Id,
                    PostId = postid,
                    CurrentAsset = postAsset.Asset,
                    Caption = postAsset.Caption,
                    ReturnUrl = returnurl
                });
            }
        }
        //HttpPost task that actually does the updating and saving of new post assets, and redirects the page
        //back to index even if it's taking some time for the changes to be saved to the database.
        [HttpPost]
        public async Task<IActionResult> EditPostAsset(PostAssetViewModel assetvm)
        {
            string returnurl = assetvm.ReturnUrl;
            var postAsset = new PostAsset
            {
                Id = assetvm.Id,
                Caption = assetvm.Caption,
            };
            if (assetvm.Asset == null)
                postAsset.Asset = assetvm.CurrentAsset;
            else
            {
                if (!string.IsNullOrEmpty(assetvm.CurrentAsset))
                    _fileManager.RemovePostAsset(assetvm.CurrentAsset);
                postAsset.Asset = _fileManager.SavePostAsset(assetvm.Asset);
            }
            if (postAsset.Id > 0)
            {
                _repo.UpdatePostAsset(postAsset);
            }
            else
            {
                var post = _repo.GetPostForAssets(assetvm.PostId);
                postAsset.Post = post;
                _repo.AddPostAsset(postAsset);
            }
            if (await _repo.SaveChangesAsync())
            {
                return LocalRedirect(returnurl);
            }
            else
                return View(postAsset);
        }
        //Http get to delete a particular post asset using its id.
        [HttpGet]
        public async Task<IActionResult> DeletePostAsset(int id, string returnurl)
        {
            _repo.DeletePostAsset(id);
            await _repo.SaveChangesAsync();
            return LocalRedirect(returnurl);
        }
        //HttpGet for admin panel portfolio assets page, shows all portfolio assets. 
        public IActionResult ManagePortfolioAssets()
        {
            var portfolioAssets = _repo.GetAllPortfolioAssets();
            return View(portfolioAssets);
        }
        //Http Get for Managing Portfolio Assets.
        [HttpGet]
        public IActionResult ManageAssetsForPortfolioItem(int id)
        {
            return View(_repo.GetPortfolioItem(id));
        }
        //HttpGet for portfolio asset edit page for the selected item. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a new portfolio item.
        [HttpGet]
        public IActionResult EditPortfolioAsset(int? id, int portfolioitemid, string returnurl)
        {
            if (id == null)
            {
                var vm = new PortfolioAssetViewModel()
                {
                    PortfolioItemId = portfolioitemid,
                    ReturnUrl = returnurl

                };
                return View(vm);
            }
            else
            {
                var portfolioAsset = _repo.GetPortfolioAsset((int)id);
                return View(new PortfolioAssetViewModel
                {
                    Id = portfolioAsset.Id,
                    PortfolioItemId = portfolioitemid,
                    CurrentAsset = portfolioAsset.Asset,
                    Caption = portfolioAsset.Caption,
                    ReturnUrl = returnurl
                });
            }
        }
        //HttpPost task that actually does the updating and saving of new portfolio item assets, and redirects the page
        //back to index even if it's taking some time for the changes to be saved to the database.
        [HttpPost]
        public async Task<IActionResult> EditPortfolioAsset(PortfolioAssetViewModel assetvm)
        {
            string returnurl = assetvm.ReturnUrl;
            var portfolioAsset = new PortfolioAsset
            {
                Id = assetvm.Id,
                Caption = assetvm.Caption
            };
            if (assetvm.Asset == null)
                portfolioAsset.Asset = assetvm.CurrentAsset;
            else
            {
                if (!string.IsNullOrEmpty(assetvm.CurrentAsset))
                    _fileManager.RemovePortfolioAsset(assetvm.CurrentAsset);
                portfolioAsset.Asset = _fileManager.SavePortfolioAsset(assetvm.Asset);
            }
            if (portfolioAsset.Id > 0)
            {
                _repo.UpdatePortfolioAsset(portfolioAsset);
            }
            else
            {
                var item = _repo.GetPortfolioItemForAssets(assetvm.PortfolioItemId);
                portfolioAsset.PortfolioItem = item;
                _repo.AddPortfolioAsset(portfolioAsset);
            }
            if (await _repo.SaveChangesAsync())
            {
                return LocalRedirect(returnurl);
            }
            else
                return View(portfolioAsset);
        }
        //Http get to delete a particular portfolio asset using its id. 
        [HttpGet]
        public async Task<IActionResult> DeletePortfolioAsset(int id, string returnurl)
        {
            _repo.DeletePortfolioAsset(id);
            await _repo.SaveChangesAsync();
            return LocalRedirect(returnurl);
        }
        //HttpGet for admin panel about page, shows all sections. 
        public IActionResult ManageAbout()
        {
            var vm = _repo.GetAllAbout();
            return View(vm);
        }
        //HttpGet for about edit page for the selected section. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a section.
        [HttpGet]
        public IActionResult EditAbout(int? id)
        {
            if (id == null)
                return View(new EditAboutViewModel());
            
            else
            {
                var about = _repo.GetAbout((int)id);
                return View(new EditAboutViewModel
                {
                    Id = about.Id,
                    Heading = about.Heading,
                    PageOrder = about.PageOrder,
                    Body = about.Body
                }); 
            }
        }
        //HttpPost task that actually does the updating and saving of about sections, and redirects the page
        //back to index even if it's taking some time for the changes to be saved to the database.
        [HttpPost]
        public async Task<IActionResult> EditAbout(EditAboutViewModel vm)
        {
            var about = new About
            {
                Id = vm.Id,
                Heading = vm.Heading,
                PageOrder = vm.PageOrder,
                Body = vm.Body
            };
            
            if (about.Id > 0)
            {
                _repo.UpdateAbout(about);
            }
            else
            {
                _repo.AddAbout(about);
            }
            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("ManageAbout");
            }
            else
                return View(about);
        }
        //Http get to delete a particular about section using its id. 
        [HttpGet]
        public async Task<IActionResult> DeleteAbout(int id)
        {
            _repo.DeleteAbout(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("ManageAbout");
        }
        //HttpGet for admin panel about assets page, shows all about assets.
        public IActionResult ManageAboutAssets()
        {
            var aboutAssets = _repo.GetAllAboutAssets();
            return View(aboutAssets);
        }
        //Http Get for Managing About Assets.
        [HttpGet]
        public IActionResult ManageAssetsForAbout(int id)
        {
            return View(_repo.GetAbout(id));
        }
        //HttpGet for about asset edit page for the selected about section. If/else statement just in case the id is null - 
        //prevents an error being thrown, instead redirects to a create a new asset.
        [HttpGet]
        public IActionResult EditAboutAsset(int? id, int aboutid, string returnurl)
        {
            if (id == null)
            {
                var vm = new AboutAssetViewModel()
                {
                    AboutId = aboutid,
                    ReturnUrl = returnurl

                };
                return View(vm);
            }
            else
            {
                var aboutAsset = _repo.GetAboutAsset((int)id);
                return View(new AboutAssetViewModel
                {
                    Id = aboutAsset.Id,
                    AboutId = aboutid,
                    CurrentAsset = aboutAsset.Asset,
                    Caption = aboutAsset.Caption,
                    ReturnUrl = returnurl
                });
            }
        }
        //HttpPost task that actually does the updating and saving of new about assets, and redirects the page
        //back to index even if it's taking some time for the changes to be saved to the database.
        [HttpPost]
        public async Task<IActionResult> EditAboutAsset(AboutAssetViewModel assetvm)
        {
            string returnurl = assetvm.ReturnUrl;
            var aboutAsset = new AboutAsset
            {
                Id = assetvm.Id,
                Caption = assetvm.Caption,
            };
            if (assetvm.Asset == null)
                aboutAsset.Asset = assetvm.CurrentAsset;
            else
            {
                if (!string.IsNullOrEmpty(assetvm.CurrentAsset))
                    _fileManager.RemoveAboutAsset(assetvm.CurrentAsset);
                aboutAsset.Asset = _fileManager.SaveAboutAsset(assetvm.Asset);
            }
            if (aboutAsset.Id > 0)
            {
                _repo.UpdateAboutAsset(aboutAsset);
            }
            else
            {
                var about = _repo.GetAboutForAssets(assetvm.AboutId);
                aboutAsset.About = about;
                _repo.AddAboutAsset(aboutAsset);
            }
            if (await _repo.SaveChangesAsync())
            {
                return LocalRedirect(returnurl);
            }
            else
                return View(aboutAsset);
        }
        //Http get to delete a particular about asset using its id.
        [HttpGet]
        public async Task<IActionResult> DeleteAboutAsset(int id, string returnurl)
        {
            _repo.DeleteAboutAsset(id);
            await _repo.SaveChangesAsync();
            return LocalRedirect(returnurl);
        }
    }
}
