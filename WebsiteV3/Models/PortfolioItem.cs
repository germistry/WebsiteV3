using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteV3.Models.Comments;

namespace WebsiteV3.Models
{
    public class PortfolioItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Introduction { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public Category Category { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<MainComment> MainComments { get; set; }
        public string SourceCodeLink { get; set; }
    }
}
