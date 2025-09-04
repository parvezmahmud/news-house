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
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            //AccountController ac= new AccountController();
            //var user = new RegisterViewModel[]
            //{
            //    new RegisterViewModel{FullName="Mr. A", Email="mra@gmail.com", Password="1234", ConfirmPassword="1234"},
            //    new RegisterViewModel{FullName="Mr. B", Email="mrb@gmail.com", Password="1234", ConfirmPassword="1234"},
            //    new RegisterViewModel{FullName="Mr. C", Email="mrc@gmail.com", Password="1234", ConfirmPassword="1234"}
            //};
            //foreach(var item in user)
            //{
            //   await ac.Register(item);
            //}
            //ApplicationUser application = new ApplicationUser();

            ////Add roles
            //var roles = new IdentityRole[]
            //{
            //    new IdentityRole{Id="1", Name="Admin"},
            //    new IdentityRole{Id="2", Name="Author"}
            //};
            //foreach (var role in roles)
            //{
            //    context.Roles.Add(role);
            //}

            ////Assign roles
            //var data = new IdentityUserRole { RoleId = "1" , UserId="asas"};
            //var roleStore = new RoleStore<IdentityRole>(context);
            //var userStore = new UserStore<ApplicationUser>(context);
            string[] roles = new string[] { "Admin", "Author" };
            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    context.Roles.Add(new IdentityRole(role));
                }
            }

            
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = new ApplicationUser[]
                {
                    new ApplicationUser{
                    Email = "mra@gmail.com",
                    UserName = "Mr.A",
                    PhoneNumber = "",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = userManager.PasswordHasher.HashPassword("1234"),
                    LockoutEnabled = true,
                    },
                    new ApplicationUser{
                    Email = "mrb@gmail.com",
                    UserName = "Mr.B",
                    PhoneNumber = "",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = userManager.PasswordHasher.HashPassword("1234"),
                    LockoutEnabled = true,
                    },
                    new ApplicationUser{
                    Email = "mrc@gmail.com",
                    UserName = "Mr.C",
                    PhoneNumber = "",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = userManager.PasswordHasher.HashPassword("1234"),
                    LockoutEnabled = true,
                    }

                };
                foreach(var item in user)
                {
                    userManager.Create(item);
                    userManager.AddToRole(item.Id, "Author");
                }
                
            




            //Application Custom Data
            var author = new Author[]
            {
                new Author {AuthorId=1, FullName="Mr. A", EmailAddress="mra@gmail.com"},
                new Author {AuthorId=2, FullName="Mr. B", EmailAddress="mrb@gmail.com" },
                new Author {AuthorId=3, FullName="Mr. C", EmailAddress="mrc@gmail.com" }

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
