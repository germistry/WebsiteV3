using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PhotoSauce.MagicScaler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Data.FileManager
{
    public class FileManager : IFileManager
    {
        private string _blogImagePath;
        private string _portfolioImagePath;

        public FileManager(IConfiguration config)
        {
            _blogImagePath = config["Path:BlogImages"];
            _portfolioImagePath = config["Path:PortfolioImages"];
        }
        //Save post image 
        public string SavePostImage(IFormFile postImage)
        {
            try
            {
                //Get the path & if path can't be saved because doesn't exist then create it
                var save_path = Path.Combine(_blogImagePath);
                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }
                //Get the mime & fileName, done this way to prevent internet explorer errors
                var mime = postImage.FileName.Substring(postImage.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";
                //Get the filestream and then save the image
                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    //Imagine processing to make files smaller, same size etc doesnt have an async method
                    MagicImageProcessor.ProcessImage(postImage.OpenReadStream(), fileStream, ImageOptions());
                }
                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }
        //Get a post image
        public FileStream PostImageStream(string postImage)
        {
            return new FileStream(Path.Combine(_blogImagePath, postImage), FileMode.Open, FileAccess.Read);
        }
        //Remove a post image 
        public void RemovePostImage(string postImage)
        {
            try
            {
                var file = Path.Combine(_blogImagePath, postImage);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //Save a portfolio item image
        public string SavePortfolioItemImage(IFormFile portfolioItemImage)
        {
            try
            {
                //Get the path & if path can't be saved because doesn't exist then create it
                var save_path = Path.Combine(_portfolioImagePath);
                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }
                //Get the mime & fileName, done this way to prevent internet explorer errors
                var mime = portfolioItemImage.FileName.Substring(portfolioItemImage.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";
                //Get the filestream and then save the image
                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    //Imagine processing to make files smaller, same size etc doesnt have an async method
                    MagicImageProcessor.ProcessImage(portfolioItemImage.OpenReadStream(), fileStream, ImageOptions());
                }
                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }
        //Get a portfolio item image
        public FileStream PortfolioItemImageStream(string portfolioItemImage)
        {
            return new FileStream(Path.Combine(_portfolioImagePath, portfolioItemImage), FileMode.Open, FileAccess.Read);
        }
        //Delete a portfolio item image
        public void RemovePortfolioImage(string portfolioItemImage)
        {
            try
            {
                var file = Path.Combine(_portfolioImagePath, portfolioItemImage);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //Imaging resizing
        private ProcessImageSettings ImageOptions() => new ProcessImageSettings
        {
            Width = 1200,
            Height = 800,
            ResizeMode = CropScaleMode.Crop,
            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 100,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420
        };
    }
}