using System.Collections.Generic;

namespace WebsiteV3.Models.PostComments
{
    public class PostMainComment : PostComment
    {
        public List<PostSubComment> SubComments { get; set; }
        public int PostId { get; set; }
        public string PostSlug { get; set; }

    }
}
