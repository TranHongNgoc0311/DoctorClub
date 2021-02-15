namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Academy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Status = c.Boolean(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAcademies",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        UserID = c.Int(),
                        AId = c.Int(),
                        Image = c.String(),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Academy", t => t.AId)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.AId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                        AccStatus = c.Int(),
                        Birthday = c.DateTime(),
                        Image = c.String(),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Awards = c.String(),
                        online = c.Boolean(),
                        Introdution = c.String(),
                        ActivePoints = c.Int(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Content = c.String(),
                        Created = c.DateTime(),
                        Updated = c.DateTime(),
                        UserId = c.Int(),
                        PostId = c.String(maxLength: 128),
                        ParrentId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.PostId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(),
                        Title = c.String(nullable: false, maxLength: 200),
                        Slug = c.String(),
                        CatId = c.Int(),
                        Content = c.String(),
                        Views = c.Int(),
                        Created = c.DateTime(),
                        Updated = c.DateTime(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CatId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CatId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Slug = c.String(),
                        description = c.String(),
                        parentId = c.Int(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserSpecializations",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        UserID = c.Int(),
                        SpId = c.Int(),
                        position = c.String(nullable: false, maxLength: 100),
                        Place = c.String(nullable: false, maxLength: 200),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Specializations", t => t.SpId)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.SpId);
            
            CreateTable(
                "dbo.Specializations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Status = c.Boolean(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CmtLikes",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        PostId = c.String(maxLength: 128),
                        UserId = c.Int(nullable: false),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 200),
                        url = c.String(),
                        Created = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserNotifications",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        NotificationId = c.String(maxLength: 128),
                        UserId = c.Int(nullable: false),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Notifications", t => t.NotificationId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.NotificationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Slug = c.String(),
                        PostId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "PostId", "dbo.Posts");
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.CmtLikes", "UserId", "dbo.Users");
            DropForeignKey("dbo.CmtLikes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.UserAcademies", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserSpecializations", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserSpecializations", "SpId", "dbo.Specializations");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Posts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Posts", "CatId", "dbo.Categories");
            DropForeignKey("dbo.UserAcademies", "AId", "dbo.Academy");
            DropIndex("dbo.Tags", new[] { "PostId" });
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropIndex("dbo.CmtLikes", new[] { "UserId" });
            DropIndex("dbo.CmtLikes", new[] { "PostId" });
            DropIndex("dbo.UserSpecializations", new[] { "SpId" });
            DropIndex("dbo.UserSpecializations", new[] { "UserID" });
            DropIndex("dbo.Posts", new[] { "CatId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.UserAcademies", new[] { "AId" });
            DropIndex("dbo.UserAcademies", new[] { "UserID" });
            DropTable("dbo.Tags");
            DropTable("dbo.UserNotifications");
            DropTable("dbo.Notifications");
            DropTable("dbo.CmtLikes");
            DropTable("dbo.Specializations");
            DropTable("dbo.UserSpecializations");
            DropTable("dbo.Categories");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.Users");
            DropTable("dbo.UserAcademies");
            DropTable("dbo.Academy");
        }
    }
}
