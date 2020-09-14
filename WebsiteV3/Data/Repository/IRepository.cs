using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models;
using WebsiteV3.Models.PostComments;
using WebsiteV3.Models.PortfolioItemComments;
using WebsiteV3.ViewModels;

namespace WebsiteV3.Data.Repository
{
    public interface IRepository
    {

        //Methods for Category
        Category GetCategory(int id);
        Category GetCategoryNoTracking(int id); //For getting category for updating posts/portfolio items without the tracking as tracking is done on the post or portfolio item.
        List<Category> GetAllCategories();
        
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
        //Methods for Posts
        Post GetPost(int id);
        List<Post> GetAllPosts();
        BlogViewModel GetAllPosts(int pageNumber, int category, string searchPosts);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void DeletePost(int id);
        
        //Methods for Portfolio Items
        PortfolioItem GetPortfolioItem(int id);
        List<PortfolioItem> GetAllPortfolioItems();
        PortfolioViewModel GetAllPortfolioItems(int pageNumber, int category, string searchItems);
        void AddPortfolioItem(PortfolioItem portfolioItem);
        void UpdatePortfolioItem(PortfolioItem portfolioItem);
        void DeletePortfolioItem(int id);
        
        //Task to save changes 
        Task<bool> SaveChangesAsync();

        //Post Maincomment Methods
        void AddPostMainComment(PostMainComment comment);
        void DeletePostMainComment(int id);
        PostMainComment GetPostMainComment(int id);

        // Post Subcomment Methods
        void AddPostSubComment(PostSubComment comment);
        void DeletePostSubComment(int id);
        PostSubComment GetPostSubComment(int id);
        
        //PortfolioItem Maincomment Methods
        void AddPortfolioItemMainComment(PortfolioItemMainComment comment);
        void DeletePortfolioItemMainComment(int id);
        PortfolioItemMainComment GetPortfolioItemMainComment(int id);

        // PortfolioItem Subcomment Methods
        void AddPortfolioItemSubComment(PortfolioItemSubComment comment);
        void DeletePortfolioItemSubComment(int id);
        PortfolioItemSubComment GetPortfolioItemSubComment(int id);


    }
}
