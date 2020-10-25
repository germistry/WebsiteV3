using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
