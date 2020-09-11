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
using WebsiteV3.Models.Comments;
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
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.User)
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                        .ThenInclude(sc => sc.User)
                .FirstOrDefault(p => p.Id == id);
        }
        public List<Post> GetAllPosts()
        {
            return _ctx.Posts
                        .Include(p => p.Category)
                        .OrderByDescending(d => d.CreatedDate)
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
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.User)
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                        .ThenInclude(sc => sc.User)
                .FirstOrDefault(p => p.Id == id);
        }
        public List<PortfolioItem> GetAllPortfolioItems()
        {
            return _ctx.PortfolioItems
                        .Include(p => p.Category)
                        .OrderByDescending(d => d.CreatedDate)
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

            var categoryList = _ctx.Categories.AsNoTracking().ToList();
            var dropDownList = new SelectList(categoryList.Select(item => new SelectListItem
            {
                Text = item.CategoryName,
                Value = item.Id.ToString()
            }).ToList(), "Value", "Text");

            return new PortfolioViewModel
            {
                CategoryId = category,
                CategoryList = dropDownList,
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
        //Subcomment methods
        public SubComment GetSubComment(int id)
        {
            return _ctx.SubComments
                    .FirstOrDefault(sc => sc.Id == id);
        }
        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }
        public void DeleteSubComment(int id)
        {
            _ctx.SubComments.Remove(GetSubComment(id));
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

       
    }
}
