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
        //Home Index View Model - getting featured stuff
        HomeIndexViewModel GetFeatures();
        //Methods for About Page
        About GetAbout(int id);
        About GetAboutForAssets(int id); //Get about for updating about assets with tracking on.
        List<About> GetAllAbout();
        void AddAbout(About about);
        void UpdateAbout(About about);
        void DeleteAbout(int id);

        //Methods for About Assets
        AboutAsset GetAboutAsset(int id);
        List<AboutAsset> GetAllAboutAssets();
        void AddAboutAsset(AboutAsset aboutAsset);
        void UpdateAboutAsset(AboutAsset aboutAsset);
        void DeleteAboutAsset(int id);

        //Methods for Category
        Category GetCategory(int id);
        Category GetCategoryNoTracking(int id); //Get category for updating posts/portfolio items without the tracking as tracking is done on the post or portfolio item.
        List<Category> GetAllCategories();
        List<Category> GetCategoryLinks();
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
        
        //Methods for Posts
        Post GetPost(int id);
        Post GetPostForAssets(int id); //Get post for updating post assets without other tables included and tracking on
        List<Post> GetAllPosts();
        List<Post> GetPostLinks();
        BlogViewModel GetAllPosts(int pageNumber, int category, string searchPosts);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void DeletePost(int id);

        //Methods for Post Assets
        PostAsset GetPostAsset(int id);
        List<PostAsset> GetAllPostAssets();
        void AddPostAsset(PostAsset postAsset);
        void UpdatePostAsset(PostAsset postAsset);
        void DeletePostAsset(int id);

        //Methods for Portfolio Items
        PortfolioItem GetPortfolioItem(int id);
        PortfolioItem GetPortfolioItemForAssets(int id); //Get portfolio item for updating portfolio assets without other tables included and tracking on
        List<PortfolioItem> GetAllPortfolioItems();
        List<PortfolioItem> GetPortfolioItemLinks();
        PortfolioViewModel GetAllPortfolioItems(int pageNumber, int category, string searchItems);
        void AddPortfolioItem(PortfolioItem portfolioItem);
        void UpdatePortfolioItem(PortfolioItem portfolioItem);
        void DeletePortfolioItem(int id);

        //Methods for Portfolio Assets
        PortfolioAsset GetPortfolioAsset(int id);
        List<PortfolioAsset> GetAllPortfolioAssets();
        void AddPortfolioAsset(PortfolioAsset portfolioAsset);
        void UpdatePortfolioAsset(PortfolioAsset portfolioAsset);
        void DeletePortfolioAsset(int id);

        //Task to save changes 
        Task<bool> SaveChangesAsync();

        //Post Maincomment Methods
        void AddPostMainComment(PostMainComment comment);
        void UpdatePostMainComment(PostMainComment comment);
        void DeletePostMainComment(int id);
        PostMainComment GetPostMainComment(int id);

        // Post Subcomment Methods
        void AddPostSubComment(PostSubComment comment);
        void DeletePostSubComment(int id);
        PostSubComment GetPostSubComment(int id);

        //PortfolioItem Maincomment Methods
        void AddPortfolioItemMainComment(PortfolioItemMainComment comment);
        void UpdatePortfolioItemMainComment(PortfolioItemMainComment comment);
        void DeletePortfolioItemMainComment(int id);
        PortfolioItemMainComment GetPortfolioItemMainComment(int id);

        // PortfolioItem Subcomment Methods
        void AddPortfolioItemSubComment(PortfolioItemSubComment comment);
        void DeletePortfolioItemSubComment(int id);
        PortfolioItemSubComment GetPortfolioItemSubComment(int id);

        //UserComments Methods
        List<PostMainComment> GetAllPostMainComments(string userId);
        List<PostSubComment> GetAllPostSubComments(string userId);
        List<PortfolioItemMainComment> GetAllPortfolioItemMainComments(string userId);
        List<PortfolioItemSubComment> GetAllPortfolioItemSubComments(string userId);

       

    }
}
