using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebsiteV3.Data;
using WebsiteV3.Models;

namespace WebsiteV3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //Dependancy Injection to bring in all middleware services using in the app
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                //Everything here surrounded in try catch incase something breaks
                try
                {
                    //context for the database
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    //userMgr handles all the identityusers 
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    //roleMgr handles all the identityroles 
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    //check to make sure database is created, apply the latest migrations just in case.  
                    context.Database.EnsureCreated();
                    //if no roles exist in db seed roles
                    if (!context.Roles.Any())
                    {
                        ContextSeed.SeedRolesAsync(userManager, roleManager).GetAwaiter().GetResult();
                    }
                    //if no users exist in db seed users and assign them to default roles
                    if (!context.Users.Any())
                    {
                        ContextSeed.SeedSuperAdminAsync(userManager, roleManager).GetAwaiter().GetResult();
                    }
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(e, "Error occurred seeding the db. Check program.cs");
                }
            }             
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddDebug();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
