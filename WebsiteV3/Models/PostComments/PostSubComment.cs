using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Models.PostComments
{
    public class PostSubComment : PostComment
    {
        public int PostMainCommentId { get; set; }
    }
}
