using System.IO;

namespace WebsiteV3.Helpers
{
    public class EmailHelper
    {
        //Little helper to build the email template.
        public static string BuildTemplate(string path, string template)
        {
            StreamReader str = new StreamReader(Path.Combine(path, template));
            string mailText = str.ReadToEnd();
            str.Close();

            return mailText;
        }
    }
}
