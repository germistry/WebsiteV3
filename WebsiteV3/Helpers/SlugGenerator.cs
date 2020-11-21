using System.Text;
using System.Text.RegularExpressions;

namespace WebsiteV3.Helpers
{
    //Slug Generator Helper for making slugs to be used in URLs for SEO purposes
    public class SlugGenerator
    {
        public static string ToSlug(string value)
        {
            //Convert to lower case 
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            value = Encoding.ASCII.GetString(bytes);

            //Replace spaces with -
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid characters 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
        
    }
}
