using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsiteV3.Models;
using WebsiteV3.Models.PostComments;
using WebsiteV3.Models.PortfolioItemComments;

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
        public DbSet<PostMainComment> PostMainComments { get; set; }
        //Table for SubComments, Inherits properties from parent class 'Comment' and has a MainCommentsId 
        //property which is the comment it is in reply to.
        public DbSet<PostSubComment> PostSubComments { get; set; }
        //Table for Main Comments, Inherits properties from parent class 'Comment' and has a list of 
        //SubComments.
        public DbSet<PortfolioItemMainComment> PortfolioItemMainComments { get; set; }
        //Table for SubComments, Inherits properties from parent class 'Comment' and has a MainCommentsId 
        //property which is the comment it is in reply to.
        public DbSet<PortfolioItemSubComment> PortfolioItemSubComments { get; set; }
        //Table of Categories
        public DbSet<Category> Categories { get; set; }
        //Table for Post Assets, images or other assets used in post content
        public DbSet<PostAsset> PostAssets { get; set; }
        //Table for Portfolio  Assets, images or other assets used in portfolio item content
        public DbSet<PortfolioAsset> PortfolioAssets { get; set; }
        //Table for about sections 
        public DbSet<About> About { get; set; }
        //Table for About Assets, images or other assets used in about page content
        public DbSet<AboutAsset> AboutAssets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}
