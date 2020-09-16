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
        private string _categoryImagePath;

        public FileManager(IConfiguration config)
        {
            _blogImagePath = config["Path:BlogImages"];
            _portfolioImagePath = config["Path:PortfolioImages"];
            _categoryImagePath = config["Path:CategoryImages"];
        }

        //Imaging resizing (for post and portfolio images)
        private ProcessImageSettings BlogPortfolioImageOptions() => new ProcessImageSettings
        {
            Width = 1200,
            Height = 800,
            ResizeMode = CropScaleMode.Crop,
            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 100,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420
        };
        //Imaging resizing (for category images)
        private ProcessImageSettings CategoryImageOptions() => new ProcessImageSettings
        {
            Width = 256,
            Height = 256,
            ResizeMode = CropScaleMode.Crop,
            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 100,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420
        };

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
                    MagicImageProcessor.ProcessImage(postImage.OpenReadStream(), fileStream, BlogPortfolioImageOptions());
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
                    MagicImageProcessor.ProcessImage(portfolioItemImage.OpenReadStream(), fileStream, BlogPortfolioImageOptions());
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
        
        //Save a category image
        public string SaveCategoryImage(IFormFile categoryImage)
        {
            try
            {
                //Get the path & if path can't be saved because doesn't exist then create it
                var save_path = Path.Combine(_categoryImagePath);
                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }
                //Get the mime & fileName, done this way to prevent internet explorer errors
                var mime = categoryImage.FileName.Substring(categoryImage.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mime}";
                //Get the filestream and then save the image
                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    //Imagine processing to make files smaller, same size etc doesnt have an async method
                    MagicImageProcessor.ProcessImage(categoryImage.OpenReadStream(), fileStream, CategoryImageOptions());
                }
                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }
        //Get a category image
        public FileStream CategoryImageStream(string categoryImage)
        {
            return new FileStream(Path.Combine(_categoryImagePath, categoryImage), FileMode.Open, FileAccess.Read);
        }
        //Delete a category image
        public void RemoveCategoryImage(string categoryImage)
        {
            try
            {
                var file = Path.Combine(_categoryImagePath, categoryImage);
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
    }
}