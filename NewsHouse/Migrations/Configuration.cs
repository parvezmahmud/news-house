namespace NewsHouse.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using NewsHouse.Controllers;
    using NewsHouse.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<NewsHouse.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected async override void Seed(NewsHouse.Models.ApplicationDbContext context)
        {
           
            string[] roles = new string[] { "Admin", "Author" };
            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    context.Roles.Add(new IdentityRole(role));
                }
            }


            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user1 = new ApplicationUser
            {
                Email = "mra@gmail.com",
                UserName = "Mr.A",
                PhoneNumber = "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = userManager.PasswordHasher.HashPassword("1234"),
                LockoutEnabled = true,
            };
            var user2 = new ApplicationUser
            {
                Email = "mrb@gmail.com",
                UserName = "Mr.B",
                PhoneNumber = "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = userManager.PasswordHasher.HashPassword("1234"),
                LockoutEnabled = true,
            };
            var user3 = new ApplicationUser
            {
                Email = "mrc@gmail.com",
                UserName = "Mr.C",
                PhoneNumber = "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = userManager.PasswordHasher.HashPassword("1234"),
                LockoutEnabled = true,
            };

            userManager.Create(user1);
            userManager.Create(user2);
            userManager.Create(user3);
            userManager.AddToRole(user1.Id, "Author");
            userManager.AddToRole(user2.Id, "Author");
            userManager.AddToRole(user3.Id, "Admin");






            //Application Custom Data
            context.Users.Attach(user1);
            context.Users.Attach(user2);
            context.Users.Attach(user3);
            var author = new Author[]   
            {
                new Author {AuthorId=1, FullName="Mr. A", EmailAddress="mra@gmail.com", User = user1},
                new Author {AuthorId=2, FullName="Mr. B", EmailAddress="mrb@gmail.com" , User=user2},
                new Author {AuthorId=3, FullName="Mr. C", EmailAddress="mrc@gmail.com" , User=user3}

            };
            var tags = new Tags[]
            {
                new Tags {TagsId=1, TagsName="trending" },
                new Tags {TagsId=2, TagsName="crime" },
                new Tags{TagsId=3, TagsName="election"},
                new Tags{TagsId=4, TagsName="protest"}
            };
            var categories = new Category[]
            {
                new Category {CategoryId=1,CategoryTitle="Bangladesh" },
                new Category {CategoryId=2, CategoryTitle="International" },
                new Category{CategoryId=3, CategoryTitle="Sports" }
            };
            context.Authors.AddRange(author);
            context.Tags.AddRange(tags);
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
    }
}
