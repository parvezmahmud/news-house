namespace NewsHouse.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserToAuthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Authors", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Authors", "User_Id");
            AddForeignKey("dbo.Authors", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Authors", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Authors", new[] { "User_Id" });
            DropColumn("dbo.Authors", "User_Id");
        }
    }
}
