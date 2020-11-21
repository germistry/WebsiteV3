using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WebsiteV3.Areas.Identity.IdentityHostingStartup))]
namespace WebsiteV3.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}