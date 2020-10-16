using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Models.PostComments
{
    public class PostMainComment : PostComment
    {
        public List<PostSubComment> SubComments { get; set; }
        public int PostId { get; set; }
        public string PostSlug { get; set; }

    }
}
