using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Enums;
using WebsiteV3.Helpers;
using WebsiteV3.Models;
using WebsiteV3.Models.PostComments;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.ViewModels;

namespace WebsiteV3.Data.Repository
{
    public class Repository : IRepository
    {
        private ApplicationDbContext _ctx;

        public Repository(ApplicationDbContext ctx)
        {
            _ctx = ctx;        
        }
        //Home index view model getting all featured stuff
        public HomeIndexViewModel GetFeatures()
        {
            IOrderedQueryable<Post> postsQuery = _ctx.Posts
                                            .Include(p => p.Category).AsNoTracking()
                                            .Where(p => p.Featured == true)
                                            .AsQueryable()
                                            .OrderByDescending(d => d.CreatedDate);
            IOrderedQueryable<PortfolioItem> portfolioItemQuery = _ctx.PortfolioItems
                                            .Include(p => p.Category).AsNoTracking()
                                            .AsQueryable()
                                            .OrderByDescending(d => d.CreatedDate);
            IOrderedQueryable<Category> categoryQuery = _ctx.Categories
                                            .Include(c => c.Posts).AsNoTracking()
                                            .Include(c => c.PortfolioItems).AsNoTracking()
                                            .AsQueryable()
                                            .OrderBy(c => c.CategoryName);

            return new HomeIndexViewModel
            {
                Posts = postsQuery.ToList(),
                PortfolioItems = portfolioItemQuery.Take(3).ToList(),
                Categories = categoryQuery.ToList()
            };
        }
        //Category methods
        public Category GetCategory(int id)
        {
            return _ctx.Categories
                    .Include(c => c.Posts)
                    .Include(c => c.PortfolioItems)
                    .FirstOrDefault(c => c.Id == id);
        }
        public Category GetCategoryNoTracking(int id)
        {
            return _ctx.Categories
                    .Include(c => c.Posts)
                    .Include(c => c.PortfolioItems)
                    .AsNoTracking()
                    .FirstOrDefault(c => c.Id == id);
        }
        public List<Category> GetAllCategories()
        {
            return _ctx.Categories
                .Include(c => c.Posts)
                .Include(c => c.PortfolioItems)
                .OrderBy(c => c.CategoryName)
                .ToList();
        }
        public void AddCategory(Category category)
        {
            _ctx.Categories.Add(category);
        }
        public void UpdateCategory(Category category)
        {
            _ctx.Categories.Update(category);
        }
        public void DeleteCategory(int id)
        {
            _ctx.Categories.Remove(GetCategory(id));
        }
        //Post methods
        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p => p.Category)
                .AsNoTracking()
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.User).AsNoTracking()
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                        .ThenInclude(sc => sc.User).AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }
        public List<Post> GetAllPosts()
        {
            return _ctx.Posts
                        .Include(p => p.Category).AsNoTracking()
                        .OrderByDescending(d => d.CreatedDate)
                        .ToList();
        }
        public List<Post> GetPostLinks()
        {
            return _ctx.Posts
                        .Include(p => p.Category).AsNoTracking()
                        .OrderByDescending(d => d.CreatedDate)
                        .Take(5)
                        .ToList();
        }
        public BlogViewModel GetAllPosts(int pageNumber, int category, string searchPosts)
        {
            IOrderedQueryable<Post> query = _ctx.Posts
                                            .Include(p => p.Category)
                                            .AsNoTracking()
                                            .AsQueryable()
                                            .OrderByDescending(d => d.CreatedDate);
            int postsCount = _ctx.Posts.Count();

            if (category > 0)
            {
                query = (IOrderedQueryable<Post>)query.Where(x => 
                                            EF.Functions.Like(x.Category.Id.ToString(), $"%{category}%"));
                postsCount = query.Count();
            }
            if (!String.IsNullOrEmpty(searchPosts))
            {
                query = (IOrderedQueryable<Post>)query.Where(x =>
                                    EF.Functions.Like(x.Title, $"%{searchPosts}%")
                                    || EF.Functions.Like(x.Body, $"%{searchPosts}%")
                                    || EF.Functions.Like(x.Description, $"%{searchPosts}%"));
                postsCount = query.Count();
            }
            //set page size here
            int pageSize = 6;
            int skipAmount = pageSize * (pageNumber - 1);
            int pageCount = (int)Math.Ceiling((double)postsCount / pageSize);

            return new BlogViewModel
            {
                CategoryId = category,
                SearchPosts = searchPosts,
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = postsCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
                Posts = query
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToList()
            };
        }
        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
        }
        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }
        public void DeletePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }
        //Portfolio methods
        public PortfolioItem GetPortfolioItem(int id)
        {
            return _ctx.PortfolioItems
                .Include(p => p.Category)
                .AsNoTracking()
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.User).AsNoTracking()
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                        .ThenInclude(sc => sc.User).AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }
        public List<PortfolioItem> GetAllPortfolioItems()
        {
            return _ctx.PortfolioItems
                        .Include(p => p.Category)
                        .AsNoTracking()
                        .OrderByDescending(d => d.CreatedDate)
                        .ToList();
        }
        public List<PortfolioItem> GetPortfolioItemLinks()
        {
            return _ctx.PortfolioItems
                        .Include(p => p.Category).AsNoTracking()
                        .OrderByDescending(d => d.CreatedDate)
                        .Take(5)
                        .ToList();
        }
        public PortfolioViewModel GetAllPortfolioItems(int pageNumber, int category, string searchItems)
        {
            IOrderedQueryable<PortfolioItem> query = _ctx.PortfolioItems
                                        .Include(p => p.Category)
                                        .AsNoTracking()
                                        .AsQueryable()
                                        .OrderByDescending(d => d.CreatedDate);
            int itemCount = _ctx.PortfolioItems.Count();

            if (category > 0)
            {
                query = (IOrderedQueryable<PortfolioItem>)query.Where(x =>
                                            EF.Functions.Like(x.Category.Id.ToString(), $"%{category}%"));
                itemCount = query.Count();
            }
            if (!String.IsNullOrEmpty(searchItems))
            {
                query = (IOrderedQueryable<PortfolioItem>)query.Where(x =>
                                    EF.Functions.Like(x.Title, $"%{searchItems}%")
                                    || EF.Functions.Like(x.Body, $"%{searchItems}%")
                                    || EF.Functions.Like(x.Description, $"%{searchItems}%"));
                itemCount = query.Count();
            }

            //set page size here
            int pageSize = 6;
            int skipAmount = pageSize * (pageNumber - 1);
            int pageCount = (int)Math.Ceiling((double)itemCount / pageSize);

            return new PortfolioViewModel
            {
                CategoryId = category,
                SearchItems = searchItems,
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = itemCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
                PortfolioItems = query
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToList()
            };
        }
        public void AddPortfolioItem(PortfolioItem portfolioItem)
        {
            _ctx.PortfolioItems.Add(portfolioItem);
        }
        public void UpdatePortfolioItem(PortfolioItem portfolioItem)
        {
            _ctx.PortfolioItems.Update(portfolioItem);
        }
        public void DeletePortfolioItem(int id)
        {
            _ctx.PortfolioItems.Remove(GetPortfolioItem(id));
        }
        //Save Changes Async
        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
        //Post Main Comments
        public void AddPostMainComment(PostMainComment comment)
        {
            _ctx.PostMainComments.Add(comment);
        }
        public void DeletePostMainComment(int id)
        {
            _ctx.PostMainComments.Remove(GetPostMainComment(id));
        }
        public PostMainComment GetPostMainComment(int id)
        {
            return _ctx.PostMainComments.AsNoTracking()
                   .FirstOrDefault(mc => mc.Id == id);
        }
        //Post Sub Comment methods
        public void AddPostSubComment(PostSubComment comment)
        {
            _ctx.PostSubComments.Add(comment);
        }
        public void DeletePostSubComment(int id)
        {
            _ctx.PostSubComments.Remove(GetPostSubComment(id));
        }
        public PostSubComment GetPostSubComment(int id)
        {
            return _ctx.PostSubComments.AsNoTracking()
                   .FirstOrDefault(sc => sc.Id == id);
        }
        //Portfolio Item Main comment methods
        public void AddPortfolioItemMainComment(PortfolioItemMainComment comment)
        {
            _ctx.PortfolioItemMainComments.Add(comment);
        }
        public void DeletePortfolioItemMainComment(int id)
        {
            _ctx.PortfolioItemMainComments.Remove(GetPortfolioItemMainComment(id));
        }
        public PortfolioItemMainComment GetPortfolioItemMainComment(int id)
        {
            return _ctx.PortfolioItemMainComments.AsNoTracking()
                   .FirstOrDefault(mc => mc.Id == id);
        }
        //Portfolio Item Sub Comment methods
        public void AddPortfolioItemSubComment(PortfolioItemSubComment comment)
        {
            _ctx.PortfolioItemSubComments.Add(comment);
        }
        public void DeletePortfolioItemSubComment(int id)
        {
            _ctx.PortfolioItemSubComments.Remove(GetPortfolioItemSubComment(id));
        }
        public PortfolioItemSubComment GetPortfolioItemSubComment(int id)
        {
            return _ctx.PortfolioItemSubComments.AsNoTracking()
                   .FirstOrDefault(sc => sc.Id == id);
        }
    }
}
