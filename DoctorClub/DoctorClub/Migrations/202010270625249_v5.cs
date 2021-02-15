namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "CatId", "dbo.Categories");
            DropIndex("dbo.Posts", new[] { "CatId" });
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Slug = c.String(),
                        CatId = c.Int(),
                        description = c.String(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CatId)
                .Index(t => t.CatId);
            
            AddColumn("dbo.Posts", "SubCatId", c => c.Int());
            CreateIndex("dbo.Posts", "SubCatId");
            AddForeignKey("dbo.Posts", "SubCatId", "dbo.SubCategories", "Id");
            DropColumn("dbo.Posts", "CatId");
            DropColumn("dbo.Categories", "parentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "parentId", c => c.Int());
            AddColumn("dbo.Posts", "CatId", c => c.Int());
            DropForeignKey("dbo.Posts", "SubCatId", "dbo.SubCategories");
            DropForeignKey("dbo.SubCategories", "CatId", "dbo.Categories");
            DropIndex("dbo.SubCategories", new[] { "CatId" });
            DropIndex("dbo.Posts", new[] { "SubCatId" });
            DropColumn("dbo.Posts", "SubCatId");
            DropTable("dbo.SubCategories");
            CreateIndex("dbo.Posts", "CatId");
            AddForeignKey("dbo.Posts", "CatId", "dbo.Categories", "Id");
        }
    }
}
