using System.Collections.Generic;
using WebsiteV3.Models;

namespace WebsiteV3.ViewModels
{
    public class BlogViewModel
    {
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public bool NextPage { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SearchPosts { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public IEnumerable<int> Pages { get; internal set; }
    }
}
