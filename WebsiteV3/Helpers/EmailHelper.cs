using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteV3.Helpers
{
    public class EmailHelper
    {
        //Little helper to build the email template.
        public static string BuildTemplate(string path, string template)
        {
            string fullPath = Path.Combine(path, template);

            StreamReader str = new StreamReader(fullPath);
            string mailText = str.ReadToEnd();
            str.Close();

            return mailText;
        }
    }
}
