using System.Collections.Generic;

namespace WebsiteV3.Models
{
    public class About
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public int PageOrder { get; set; }
        public string Body { get; set; }
        public List<AboutAsset> AboutAssets { get; set; }
    }
}
