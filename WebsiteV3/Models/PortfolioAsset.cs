namespace WebsiteV3.Models
{
    public class PortfolioAsset
    {
        public int Id { get; set; }
        public string Asset { get; set; }
        public PortfolioItem PortfolioItem { get; set; }
        public string Caption { get; set; }
    }
}
