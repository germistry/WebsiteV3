﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV3.Helpers;
using WebsiteV3.Models;
using WebsiteV3.Models.PostComments;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.ViewModels;
using WebsiteV3.Data.FileManager;

namespace WebsiteV3.Data.Repository
{
    public class Repository : IRepository
    {
        private ApplicationDbContext _ctx;
        private IFileManager _fileManager;

        public Repository(ApplicationDbContext ctx, IFileManager fileManager)
        {
            _ctx = ctx;
            _fileManager = fileManager;
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
        public List<Category> GetCategoryLinks()
        {
            return _ctx.Categories
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
            var query = GetCategory(id);
            string imageFileName = query.Image;
            if (!String.IsNullOrEmpty(imageFileName))
            {
                try
                {
                    _fileManager.RemoveCategoryImage(imageFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            _ctx.Categories.Remove(query);
        }
        //Post methods
        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p => p.Category)
                .AsNoTracking()
                .Include(p => p.PostAssets).AsNoTracking()
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.User).AsNoTracking()
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                        .ThenInclude(sc => sc.User).AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }
        public Post GetPostForAssets(int id)
        {
            return _ctx.Posts
                       .Include(p => p.PostAssets)
                       .FirstOrDefault(p => p.Id == id);
        }
        public List<Post> GetAllPosts()
        {
            return _ctx.Posts
                        .Include(p => p.Category).AsNoTracking()
                        .Include(p => p.MainComments).AsNoTracking()
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
            var queryPost = GetPost(id);
            string imageFileName = queryPost.Image;
            if (!String.IsNullOrEmpty(imageFileName))
            {
                try
                {
                    _fileManager.RemovePostImage(imageFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            var queryAssets = queryPost.PostAssets;
            foreach (var asset in queryAssets)
            {
                string assetFileName = asset.Asset;
                if (!String.IsNullOrEmpty(assetFileName))
                {
                    try
                    {
                        _fileManager.RemovePostAsset(assetFileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                _ctx.PostAssets.Remove(asset);
            }
            _ctx.Posts.Remove(queryPost);
        }

        //Portfolio methods
        public PortfolioItem GetPortfolioItem(int id)
        {
            return _ctx.PortfolioItems
                .Include(p => p.Category)
                .AsNoTracking()
                .Include(p => p.PortfolioAssets).AsNoTracking()
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.User).AsNoTracking()
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                        .ThenInclude(sc => sc.User).AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }
        public PortfolioItem GetPortfolioItemForAssets(int id)
        {
            return _ctx.PortfolioItems
                       .Include(p => p.PortfolioAssets)
                       .FirstOrDefault(p => p.Id == id);
        }
        public List<PortfolioItem> GetAllPortfolioItems()
        {
            return _ctx.PortfolioItems
                        .Include(p => p.Category).AsNoTracking()
                        .Include(p => p.MainComments).AsNoTracking()
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
            var queryItem = GetPortfolioItem(id);
            string imageFileName = queryItem.Image;
            if (!String.IsNullOrEmpty(imageFileName))
            {
                try
                {
                    _fileManager.RemovePortfolioImage(imageFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            var queryAssets = queryItem.PortfolioAssets;
            foreach (var asset in queryAssets)
            {
                string assetFileName = asset.Asset;
                if (!String.IsNullOrEmpty(assetFileName))
                {
                    try
                    {
                        _fileManager.RemovePortfolioAsset(assetFileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                _ctx.PortfolioAssets.Remove(asset);
            }
            _ctx.PortfolioItems.Remove(queryItem);
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
        //Used only to change a post main comment to a blank if user/admin want to delete the comment and 
        //it has sub comments, so sub comments aren't lost on a thread. 
        public void UpdatePostMainComment(PostMainComment comment)
        {
            _ctx.PostMainComments.Update(comment);
        }
        public void DeletePostMainComment(int id)
        {
            _ctx.PostMainComments.Remove(GetPostMainComment(id));
        }
        public PostMainComment GetPostMainComment(int id)
        {
            return _ctx.PostMainComments.AsNoTracking()
                        .Include(mc => mc.SubComments).AsNoTracking()
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
        //Used only to change a portfolio main comment to a blank if user/admin want to delete the comment and 
        //it has sub comments, so sub comments aren't lost on a thread. 
        public void UpdatePortfolioItemMainComment(PortfolioItemMainComment comment)
        {
            _ctx.PortfolioItemMainComments.Update(comment);
        }
        public void DeletePortfolioItemMainComment(int id)
        {
            _ctx.PortfolioItemMainComments.Remove(GetPortfolioItemMainComment(id));
        }
        public PortfolioItemMainComment GetPortfolioItemMainComment(int id)
        {
            return _ctx.PortfolioItemMainComments.AsNoTracking()
                        .Include(mc => mc.SubComments).AsNoTracking()
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
        //User comment methods
        public List<PostMainComment> GetAllPostMainComments(string userId)
        {
            IOrderedQueryable<PostMainComment> query = _ctx.PostMainComments
                                                            .Include(mc => mc.User).AsNoTracking()
                                                            .Include(mc => mc.SubComments).AsNoTracking()
                                                            .AsQueryable()
                                                            .OrderByDescending(d => d.CreatedDate);
            if (!String.IsNullOrEmpty(userId))
            {
                query = (IOrderedQueryable<PostMainComment>)query.Where(x =>
                                    EF.Functions.Like(x.UserId, $"%{userId}%"));
            }
            return query.ToList();
        }
        public List<PostSubComment> GetAllPostSubComments(string userId)
        {
            IOrderedQueryable<PostSubComment> query = _ctx.PostSubComments
                                                        .Include(sc => sc.User).AsNoTracking()
                                                        .AsQueryable()
                                                        .OrderByDescending(d => d.CreatedDate);
            if (!String.IsNullOrEmpty(userId))
            {
                query = (IOrderedQueryable<PostSubComment>)query.Where(x =>
                                    EF.Functions.Like(x.UserId, $"%{userId}%"));
            }
            return query.ToList();
        }
        public List<PortfolioItemMainComment> GetAllPortfolioItemMainComments(string userId)
        {
            IOrderedQueryable<PortfolioItemMainComment> query = _ctx.PortfolioItemMainComments
                                                            .Include(mc => mc.User).AsNoTracking()
                                                            .Include(mc => mc.SubComments).AsNoTracking()
                                                            .AsQueryable()
                                                            .OrderByDescending(d => d.CreatedDate);
            if (!String.IsNullOrEmpty(userId))
            {
                query = (IOrderedQueryable<PortfolioItemMainComment>)query.Where(x =>
                                    EF.Functions.Like(x.UserId, $"%{userId}%"));
            }
            return query.ToList();
        }
        public List<PortfolioItemSubComment> GetAllPortfolioItemSubComments(string userId)
        {
            IOrderedQueryable<PortfolioItemSubComment> query = _ctx.PortfolioItemSubComments
                                                        .Include(sc => sc.User).AsNoTracking()
                                                        .AsQueryable()
                                                        .OrderByDescending(d => d.CreatedDate);
            if (!String.IsNullOrEmpty(userId))
            {
                query = (IOrderedQueryable<PortfolioItemSubComment>)query.Where(x =>
                                    EF.Functions.Like(x.UserId, $"%{userId}%"));
            }
            return query.ToList();
        }
        //Post Asset Methods
        public PostAsset GetPostAsset(int id)
        {
            return _ctx.PostAssets
                .Include(p => p.Post).AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }

        public List<PostAsset> GetAllPostAssets()
        {
            return _ctx.PostAssets
                        .Include(p => p.Post).AsNoTracking()
                        .OrderBy(a => a.Asset)
                        .ToList();
        }

        public void AddPostAsset(PostAsset postAsset)
        {
            _ctx.PostAssets.Add(postAsset);
        }

        public void UpdatePostAsset(PostAsset postAsset)
        {
            _ctx.PostAssets.Update(postAsset);
        }

        public void DeletePostAsset(int id)
        {
            var query = GetPostAsset(id);
            string assetFileName = query.Asset;
            if (!String.IsNullOrEmpty(assetFileName))
            {
                try
                {
                    _fileManager.RemovePostAsset(assetFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            _ctx.PostAssets.Remove(query);
        }
        //Portfolio Asset Methods
        public PortfolioAsset GetPortfolioAsset(int id)
        {
            return _ctx.PortfolioAssets
                .Include(p => p.PortfolioItem).AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }

        public List<PortfolioAsset> GetAllPortfolioAssets()
        {
            return _ctx.PortfolioAssets
                        .Include(p => p.PortfolioItem).AsNoTracking()
                        .OrderBy(a => a.Asset)
                        .ToList();
        }

        public void AddPortfolioAsset(PortfolioAsset portfolioAsset)
        {
            _ctx.PortfolioAssets.Add(portfolioAsset);
        }

        public void UpdatePortfolioAsset(PortfolioAsset portfolioAsset)
        {
            _ctx.PortfolioAssets.Update(portfolioAsset);
        }

        public void DeletePortfolioAsset(int id)
        {
            var query = GetPortfolioAsset(id);
            string assetFileName = query.Asset;
            if (!String.IsNullOrEmpty(assetFileName))
            {
                try
                {
                    _fileManager.RemovePortfolioAsset(assetFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            _ctx.PortfolioAssets.Remove(query);
        }
        //About methods
        public About GetAbout(int id)
        {
            return _ctx.About
                .Include(p => p.AboutAssets).AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }

        public List<About> GetAllAbout()
        {
            return _ctx.About
                        .OrderBy(a => a.PageOrder)
                        .ToList();
        }
        public About GetAboutForAssets(int id)
        {
           return _ctx.About
                       .Include(p => p.AboutAssets)
                       .FirstOrDefault(p => p.Id == id);
        }
        public void AddAbout(About about)
        {
            _ctx.About.Add(about);
        }

        public void UpdateAbout(About about)
        {
            _ctx.About.Update(about);
        }

        public void DeleteAbout(int id)
        {
            var queryAbout = GetAbout(id);
            var queryAssets = queryAbout.AboutAssets;
           
            foreach (var asset in queryAssets)
            {
                string assetFileName = asset.Asset;
                if (!String.IsNullOrEmpty(assetFileName))
                {
                    try
                    {
                        _fileManager.RemoveAboutAsset(assetFileName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                _ctx.AboutAssets.Remove(asset);
            }; 
            _ctx.About.Remove(queryAbout);
        }
        //Methods for about assets
        public AboutAsset GetAboutAsset(int id)
        {
            return _ctx.AboutAssets
                .Include(p => p.About).AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }

        public List<AboutAsset> GetAllAboutAssets()
        {
            return _ctx.AboutAssets
                        .Include(p => p.About).AsNoTracking()
                        .OrderBy(a => a.Asset)
                        .ToList();
        }

        public void AddAboutAsset(AboutAsset aboutAsset)
        {
            _ctx.AboutAssets.Add(aboutAsset);
        }

        public void UpdateAboutAsset(AboutAsset aboutAsset)
        {
            _ctx.AboutAssets.Update(aboutAsset);
        }

        public void DeleteAboutAsset(int id)
        {
            var query = GetAboutAsset(id);
            string assetFileName = query.Asset;
            if (!String.IsNullOrEmpty(assetFileName))
            {
                try
                {
                    _fileManager.RemoveAboutAsset(assetFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            _ctx.AboutAssets.Remove(query);
        }
    }
}
