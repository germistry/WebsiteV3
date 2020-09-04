using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteV3.Models;
using WebsiteV3.Models.Comments;

namespace WebsiteV3.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Basically a table of individual posts, has a list of MainComments.  
        public DbSet<Post> Posts { get; set; }
        //A Table of individual portfolio items, has a list of Main Comments.
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        //Table for Main Comments, Inherits properties from parent class 'Comment' and has a list of 
        //SubComments.
        public DbSet<MainComment> MainComments { get; set; }
        //Table for SubComments, Inherits properties from parent class 'Comment' and has a MainCommentsId 
        //property which is the comment it is in reply to.
        public DbSet<SubComment> SubComments { get; set; }
        //Table of Categories
        public DbSet<Category> Categories { get; set; }
    }
}
