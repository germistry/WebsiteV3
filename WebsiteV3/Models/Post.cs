using System;
using System.Collections.Generic;
using WebsiteV3.Models.PostComments;

namespace WebsiteV3.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Introduction { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public Category Category { get; set; }
        public bool Featured { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<PostMainComment> MainComments { get; set; }
        public bool CommentsAllowed { get; set; } = true;
        public List<PostAsset> PostAssets { get; set; }

    }
}
