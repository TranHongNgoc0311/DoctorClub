namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tags", "PostId", "dbo.Posts");
            DropIndex("dbo.Tags", new[] { "PostId" });
            CreateTable(
                "dbo.PostTags",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        TagId = c.String(maxLength: 128),
                        PostId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Posts", t => t.PostId)
                .ForeignKey("dbo.Tags", t => t.TagId)
                .Index(t => t.TagId)
                .Index(t => t.PostId);
            
            DropColumn("dbo.Tags", "PostId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "PostId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.PostTags", "TagId", "dbo.Tags");
            DropForeignKey("dbo.PostTags", "PostId", "dbo.Posts");
            DropIndex("dbo.PostTags", new[] { "PostId" });
            DropIndex("dbo.PostTags", new[] { "TagId" });
            DropTable("dbo.PostTags");
            CreateIndex("dbo.Tags", "PostId");
            AddForeignKey("dbo.Tags", "PostId", "dbo.Posts", "Id");
        }
    }
}
