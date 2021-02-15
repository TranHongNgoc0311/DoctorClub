namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CmtLikes", "PostId", "dbo.Posts");
            DropIndex("dbo.CmtLikes", new[] { "PostId" });
            AddColumn("dbo.CmtLikes", "commentId", c => c.String(maxLength: 128));
            CreateIndex("dbo.CmtLikes", "commentId");
            AddForeignKey("dbo.CmtLikes", "commentId", "dbo.Comments", "Id");
            DropColumn("dbo.CmtLikes", "PostId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CmtLikes", "PostId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.CmtLikes", "commentId", "dbo.Comments");
            DropIndex("dbo.CmtLikes", new[] { "commentId" });
            DropColumn("dbo.CmtLikes", "commentId");
            CreateIndex("dbo.CmtLikes", "PostId");
            AddForeignKey("dbo.CmtLikes", "PostId", "dbo.Posts", "Id");
        }
    }
}
