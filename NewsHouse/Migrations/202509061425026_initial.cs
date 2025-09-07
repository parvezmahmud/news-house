namespace NewsHouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        EmailAddress = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        NewsBody = c.String(),
                        HeaderImage = c.String(),
                        IsArchived = c.Boolean(nullable: false),
                        Published = c.DateTime(nullable: false),
                        Author_AuthorId = c.Int(),
                    })
                .PrimaryKey(t => t.NewsId)
                .ForeignKey("dbo.Authors", t => t.Author_AuthorId)
                .Index(t => t.Author_AuthorId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryTitle = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagsId = c.Int(nullable: false, identity: true),
                        TagsName = c.String(),
                    })
                .PrimaryKey(t => t.TagsId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CategoryNews",
                c => new
                    {
                        Category_CategoryId = c.Int(nullable: false),
                        News_NewsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Category_CategoryId, t.News_NewsId })
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.News", t => t.News_NewsId, cascadeDelete: true)
                .Index(t => t.Category_CategoryId)
                .Index(t => t.News_NewsId);
            
            CreateTable(
                "dbo.TagsNews",
                c => new
                    {
                        Tags_TagsId = c.Int(nullable: false),
                        News_NewsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tags_TagsId, t.News_NewsId })
                .ForeignKey("dbo.Tags", t => t.Tags_TagsId, cascadeDelete: true)
                .ForeignKey("dbo.News", t => t.News_NewsId, cascadeDelete: true)
                .Index(t => t.Tags_TagsId)
                .Index(t => t.News_NewsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TagsNews", "News_NewsId", "dbo.News");
            DropForeignKey("dbo.TagsNews", "Tags_TagsId", "dbo.Tags");
            DropForeignKey("dbo.CategoryNews", "News_NewsId", "dbo.News");
            DropForeignKey("dbo.CategoryNews", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.News", "Author_AuthorId", "dbo.Authors");
            DropIndex("dbo.TagsNews", new[] { "News_NewsId" });
            DropIndex("dbo.TagsNews", new[] { "Tags_TagsId" });
            DropIndex("dbo.CategoryNews", new[] { "News_NewsId" });
            DropIndex("dbo.CategoryNews", new[] { "Category_CategoryId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.News", new[] { "Author_AuthorId" });
            DropTable("dbo.TagsNews");
            DropTable("dbo.CategoryNews");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tags");
            DropTable("dbo.Categories");
            DropTable("dbo.News");
            DropTable("dbo.Authors");
        }
    }
}
