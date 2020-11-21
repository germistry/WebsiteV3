namespace WebsiteV3.Models
{
    public class PostAsset
    {
        public int Id { get; set; }
        public string Asset { get; set; }
        public Post Post { get; set; }
        public string Caption { get; set; }
    }
}
