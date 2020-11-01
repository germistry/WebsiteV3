using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using WebsiteV3.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebsiteV3.Data.Repository;
using WebsiteV3.Data.FileManager;
using Microsoft.AspNetCore.Mvc;
using WebsiteV3.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace WebsiteV3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => {
                loggingBuilder.AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
                loggingBuilder.AddDebug();
            });

            //Db context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                //todo - disable for production DONE
                options.EnableSensitiveDataLogging(false);
            });
            //Identity & Role Setup
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                //todo PRODUCTION- change to all true when deployed
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddRoles<IdentityRole>()
                .AddDefaultUI()
                //Add db context for identity
                .AddEntityFrameworkStores<ApplicationDbContext>()
                //Default token providers such as user submits a verify email and have to be able to accept token
                .AddDefaultTokenProviders();
               
            //Makes the database repository available to the program
            services.AddTransient<IRepository, Repository>();
            //Makes the filemanager for images available to program
            services.AddTransient<IFileManager, FileManager>();

            //Email config using netcore mailkit (not just mailkit pkg)
            var mailKitOptions = Configuration.GetSection("EmailConfiguration").Get<MailKitOptions>();
            services.AddMailKit(config => config.UseMailKit(mailKitOptions));
                                             
            services.Configure<CookiePolicyOptions>(options =>
            {
                //want consent cookie to be checked to ensure user agrees to privacy policy
                options.CheckConsentNeeded = context => true;
                //cookies have to be secure
                options.Secure = CookieSecurePolicy.Always;
                //cookie samesitemode changed at runtime by other middleware
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //http strict transport security = want to be able to be added to Hsts Preloader & of course security
            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);
                options.IncludeSubDomains = true;
                options.Preload = true;
            });
            services.AddAntiforgery(options =>
            {
                //this is set expressly in content headers 
                options.SuppressXFrameOptionsHeader = true;
            });
            services.ConfigureApplicationCookie(o => {
                o.ExpireTimeSpan = TimeSpan.FromDays(5);
                o.SlidingExpiration = true;
            });
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));
            services.AddAuthentication()
                //todo - FUTURE RELEASE add twitter authenication? 
                //todo - PRODUCTION set up facebook verification for deployment
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.AccessDeniedPath = "/Identity/Account/ExternalLoginUserDenied";
                });

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Monthly", new CacheProfile { Duration = 60 * 60 * 24 * 7 * 4 });
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. Change this for production scenarios. (set for 365 days)
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                if (!context.Response.Headers.ContainsKey("Header-Name"))
                {
                    //this header just to be be used as the key for this if statement
                    context.Response.Headers.Add("Header-Name", "Header-Value");
                    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
                    context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'");
                    context.Response.Headers.Add("Referrer-Policy", "no-referrer-when-downgrade");
                    //Todo - need to remove all inline js to enable this security policy, future release
                    context.Response.Headers.Add("Content-Security-Policy-Report-Only", "default-src 'self'; script-src 'self' https://cdnjs.cloudflare.com https://ajax.googleapis.com https://ajax.aspnetcdn.com; report-uri /cspreport");
                    //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com https://ajax.googleapis.com https://ajax.aspnetcdn.com");
                }
                await next();
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{slug?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
