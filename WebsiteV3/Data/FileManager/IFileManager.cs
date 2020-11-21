using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebsiteV3.Data.FileManager
{
    public interface IFileManager
    {

        //Methods for Post Assets 
        string SavePostAsset(IFormFile postAsset);
        FileStream PostAssetImageStream(string postAsset);
        FileStream PostAssetFileStream(string postAsset);
        void RemovePostAsset(string postAsset);

        //Methods for PortfolioAssets
        string SavePortfolioAsset(IFormFile portfolioAsset);
        FileStream PortfolioAssetStream(string portfolioAsset);
        FileStream PortfolioAssetFileStream(string portfolioAsset);
        void RemovePortfolioAsset(string portfolioAsset);

        //Methods for posts
        string SavePostImage(IFormFile postImage);
        FileStream PostImageStream(string postImage);
        void RemovePostImage(string postImage);

        //Methods for portfolio items
        string SavePortfolioItemImage(IFormFile portfolioItemImage);
        FileStream PortfolioItemImageStream(string portfolioItemImage);     
        void RemovePortfolioImage(string portfolioItemImage);
        
        //Methods for categories
        string SaveCategoryImage(IFormFile categoryImage);
        FileStream CategoryImageStream(string categoryImage);
        void RemoveCategoryImage(string categoryImage);
        //Methods for about assets 
        string SaveAboutAsset(IFormFile aboutAsset);
        FileStream AboutAssetStream(string aboutAsset);
        void RemoveAboutAsset(string aboutAsset);

    }
}
