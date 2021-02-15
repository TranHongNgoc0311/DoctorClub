namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Posts", "SubCatId", "dbo.SubCategories");
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropIndex("dbo.Posts", new[] { "SubCatId" });
            AlterColumn("dbo.Academy", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Posts", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "SubCatId", c => c.Int(nullable: false));
            AlterColumn("dbo.Posts", "Views", c => c.Int(nullable: false));
            AlterColumn("dbo.SubCategories", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Categories", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Specializations", "Status", c => c.Boolean(nullable: false));
            AlterColumn("dbo.CmtLikes", "Status", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Posts", "UserId");
            CreateIndex("dbo.Posts", "SubCatId");
            AddForeignKey("dbo.Posts", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Posts", "SubCatId", "dbo.SubCategories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "SubCatId", "dbo.SubCategories");
            DropForeignKey("dbo.Posts", "UserId", "dbo.Users");
            DropIndex("dbo.Posts", new[] { "SubCatId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            AlterColumn("dbo.CmtLikes", "Status", c => c.Boolean());
            AlterColumn("dbo.Specializations", "Status", c => c.Boolean());
            AlterColumn("dbo.Categories", "Status", c => c.Boolean());
            AlterColumn("dbo.SubCategories", "Status", c => c.Boolean());
            AlterColumn("dbo.Posts", "Views", c => c.Int());
            AlterColumn("dbo.Posts", "SubCatId", c => c.Int());
            AlterColumn("dbo.Posts", "UserId", c => c.Int());
            AlterColumn("dbo.Academy", "Status", c => c.Boolean());
            CreateIndex("dbo.Posts", "SubCatId");
            CreateIndex("dbo.Posts", "UserId");
            AddForeignKey("dbo.Posts", "SubCatId", "dbo.SubCategories", "Id");
            AddForeignKey("dbo.Posts", "UserId", "dbo.Users", "Id");
        }
    }
}
