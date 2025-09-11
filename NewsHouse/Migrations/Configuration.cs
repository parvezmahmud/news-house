namespace NewsHouse.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using NewsHouse.Controllers;
    using NewsHouse.Models;
    using System;
    using System.Collections.Generic;
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

        protected override void Seed(NewsHouse.Models.ApplicationDbContext context)
        {
           
            string[] roles = new string[] { "Admin","Manager", "Author" };
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
                UserName = "mra@gmail.com",
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
                UserName = "mrb@gmail.com",
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
                UserName = "mrc@gmail.com",
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
            userManager.AddToRole(user2.Id, "Manager");
            userManager.AddToRole(user3.Id, "Admin");

            context.Users.Attach(user1);
            context.Users.Attach(user2);
            context.Users.Attach(user3);
            Author author1 = new Author { AuthorId = 1, FullName = "Mr. A", EmailAddress = "mra@gmail.com", User = user1 };
            Author author2 = new Author { AuthorId = 2, FullName = "Mr. B", EmailAddress = "mrb@gmail.com", User = user2 };
            Author author3 = new Author { AuthorId = 3, FullName = "Admin", EmailAddress = "mrc@gmail.com", User = user3 };



            Tags tag1 = new Tags { TagsId = 1, TagsName = "trending" };
            Tags tag2 = new Tags { TagsId = 2, TagsName = "crime" };
            Tags tag3 = new Tags { TagsId = 3, TagsName = "election" };
            Tags tag4 = new Tags { TagsId = 4, TagsName = "protest" };
            Tags tag5 = new Tags { TagsId = 5, TagsName = "politics" };

            var tags = new List<Tags>();
            var tagsSecond = new List<Tags>();

            Category cat1 = new Category { CategoryId = 1, CategoryTitle = "Bangladesh" };
            Category cat2 = new Category { CategoryId = 2, CategoryTitle = "International" };
            Category cat3 = new Category { CategoryId = 3, CategoryTitle = "Sports" };
            Category cat4 =  new Category { CategoryId = 4, CategoryTitle = "Financial" };
            var cats = new List<Category>();
            var categories = new List<Category>();
            cats.Add(cat1);
            cats.Add(cat2);
            cats.Add(cat3);
            categories.Add(cat4);
            categories.Add(cat2);

            tags.Add(tag1);
            tags.Add(tag2);
            tags.Add(tag3);
            tagsSecond.Add(tag5);
            tagsSecond.Add(tag4);

            News news1 = new News {
                NewsId=1, 
                Title="Treaty Signed between two Giants",
                Author=author3, 
                HeaderImage= "/Images/638929425090067541.png", 
                IsArchived=false, 
                NewsBody= "Bangladesh is one of the most climate-vulnerable countries in the world, despite having almost no role in creating the problem. The country contributes less than 0.5% of global carbon emissions, yet it consistently ranks among the top ten nation’s most at risk in the Global Climate Risk Index. Rising seas, cyclones, prolonged floods, salinity intrusion, erratic rainfall, and deadly heat waves are not distant threats here -- they are daily realities. Whole communities have already lost homes, farmland, and safe water to disasters that strike with alarming regularity.",
                Categories = cats,
                Tags=tags,
                Published=DateTime.Now
            };
            News news2 = new News
            {
                NewsId = 2,
                Title = "Climate Justice in Bangladesh",
                Author = author3,
                HeaderImage = "/Images/Climate-caf3854657c1553fdd5ba096.jpg",
                IsArchived = true,
                NewsBody = "Bangladesh is one of the most climate-vulnerable countries in the world, despite having almost no role in creating the problem. The country contributes less than 0.5% of global carbon emissions, yet it consistently ranks among the top ten nation’s most at risk in the Global Climate Risk Index. Rising seas, cyclones, prolonged floods, salinity intrusion, erratic rainfall, and deadly heat waves are not distant threats here -- they are daily realities. Whole communities have already lost homes, farmland, and safe water to disasters that strike with alarming regularity.",
                Categories = categories,
                Tags = tags,
                Published = DateTime.Now
            };
            News news3 = new News
            {
                NewsId = 3,
                Title = "Climate Justice in Bangladesh",
                Author = author3,
                HeaderImage = "/Images/Climate-caf3854657c1553fdd5ba096.jpg",
                IsArchived = true,
                NewsBody = "All Bangladeshi nationals currently living or stranded in Nepal have been strongly advised to remain indoors and stay at their respective homes or hotels.",
                Tags = tagsSecond,
                Categories = categories,
                Published = DateTime.Now
            };
            News news8 = new News
            {
                NewsId = 10,
                Title = "Heatwave Emergency in India",
                Author = author3,
                HeaderImage = "/Images/India_heat_wave_610.jpg",
                IsArchived = false,
                NewsBody = "The Indian Meteorological Department has issued an urgent heatwave warning across several states. Citizens are advised to stay hydrated and avoid outdoor activities during peak heat hours.",
                Tags = tagsSecond,
                Categories = cats,
                Published = DateTime.Now
            };

            News news9 = new News
            {
                NewsId = 4,
                Title = "Floods Hit Southern Europe",
                Author = author3,
                HeaderImage = "/Images/4525587-262460170.jpg",
                IsArchived = false,
                NewsBody = "Severe flooding has caused widespread damage across southern Europe, with several towns and villages submerged. Rescue operations are underway as people are evacuated from affected areas.",
                Tags = tags,
                Categories = categories,
                Published = DateTime.Now
            };

            News news10 = new News
            {
                NewsId = 5,
                Title = "Wildfires Ravage California Forests",
                Author = author3,
                HeaderImage = "/Images/california-fire.jpg",
                IsArchived = true,
                NewsBody = "Wildfires in California have reached unprecedented levels, forcing thousands of residents to flee their homes. The government has declared a state of emergency to facilitate aid and firefighting efforts.",
                Tags = tagsSecond,
                Categories = categories,
                Published = DateTime.Now
            };

            News news4 = new News
            {
                NewsId = 6,
                Title = "Deforestation Crisis in the Amazon",
                Author = author3,
                HeaderImage = "/Images/deforestation-in-amazon.jpg",
                IsArchived = false,
                NewsBody = "Deforestation rates in the Amazon have increased by 30% this year. Environmentalists are raising alarms as critical habitats for biodiversity continue to be destroyed at an alarming rate.",
                Tags = tags,
                Categories = cats,
                Published = DateTime.Now
            };

            News news5 = new News
            {
                NewsId = 7,
                Title = "Earthquake Strikes Turkey",
                Author = author3,
                HeaderImage = "/Images/file-20240206-30-zsfahz.jpg",
                IsArchived = false,
                NewsBody = "A powerful earthquake with a magnitude of 7.2 struck Turkey today, causing significant destruction in the city of Izmir. Rescue teams are working to find survivors amidst the rubble.",
                Tags = tagsSecond,
                Categories = cats,
                Published = DateTime.Now
            };

            News news6 = new News
            {
                NewsId = 8,
                Title = "Cyclone Hits the Philippines",
                Author = author3,
                HeaderImage = "/Images/hkg9172232-copy.jpg",
                IsArchived = true,
                NewsBody = "The Philippines is bracing for the impact of a powerful cyclone heading towards the eastern coastline. Authorities have issued evacuation orders as heavy rain and strong winds are expected to cause significant damage.",
                Tags = tags,
                Categories = categories,
                Published = DateTime.Now
            };

            News news7 = new News
            {
                NewsId = 9,
                Title = "Rising Sea Levels in Maldives",
                Author = author3,
                HeaderImage = "/Images/maldives-sea.jpg",
                IsArchived = false,
                NewsBody = "The Maldives is facing a growing crisis as rising sea levels threaten to submerge much of the island nation. Local authorities are exploring innovative solutions to protect the country’s infrastructure and inhabitants.",
                Tags = tagsSecond,
                Categories = cats,
                Published = DateTime.Now
            };


            context.Authors.Add(author1);
            context.Authors.Add(author2);
            context.Authors.Add(author3);
            context.Tags.Add(tag1);
            context.Tags.Add(tag2);
            context.Tags.Add(tag3);
            context.Tags.Add(tag4);
            context.Tags.Add(tag5);
            context.Categories.Add(cat1);
            context.Categories.Add(cat2);
            context.Categories.Add(cat3);
            context.Categories.Add(cat4);
            context.News.Add(news1);
            context.News.Add(news2);
            context.News.Add(news3);
            context.News.Add(news4);
            context.News.Add(news5);
            context.News.Add(news6);
            context.News.Add(news7);
            context.News.Add(news8);
            context.News.Add(news9);
            context.News.Add(news10);
            context.SaveChanges();
        }
    }
}
