using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebsiteV3.Data;

namespace WebsiteV3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //Everything here surrounded in try catch incase something breaks
            try
            {
                //Dependancy Injection to bring in all middleware services using in the app
                var scope = host.Services.CreateScope();
                //context for the database
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                //userMgr handles all the identityusers 
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                //roleMgr handles all the identityroles 
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //check to make sure database is created, apply the latest migrations just in case.  
                context.Database.EnsureCreated();
                //if there are no roles seed the admin role of 'Admin'
                var adminRole = new IdentityRole("Admin");
                if (!context.Roles.Any())
                {
                    //Create a role
                    roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
                }
                //if there are no users seed a user of 'admin' 
                if (!context.Users.Any(u => u.UserName == "admin@test.com"))
                {
                    //Create an admin
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin@test.com",
                        //Default identity uses the email address as username 
                        Email = "admin@test.com",
                        EmailConfirmed = true
                    };
                    //assign the super secret password of 'password' lol
                    var result = userManager.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                    //add the role to user
                    userManager.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
