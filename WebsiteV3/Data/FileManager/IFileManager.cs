using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Data.FileManager
{
    public interface IFileManager
    {
        //Methods for posts
        string SavePostImage(IFormFile postImage);

        FileStream PostImageStream(string postImage);

        //Methods for portfolio items
        string SavePortfolioItemImage(IFormFile portfolioItemImage);

        FileStream PortfolioItemImageStream(string portfolioItemImage);

        void RemovePostImage(string postImage);

        void RemovePortfolioImage(string portfolioItemImage);
    }
}
