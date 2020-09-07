using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models;
using WebsiteV3.Models.Comments;
using WebsiteV3.ViewModels;

namespace WebsiteV3.Data.Repository
{
    public interface IRepository
    {

        //Methods for Category
        Category GetCategory(int id);
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
        //Subcomment Methods
        void AddSubComment(SubComment comment);
        void DeleteSubComment(int id);
        SubComment GetSubComment(int id);

        
    }
}
