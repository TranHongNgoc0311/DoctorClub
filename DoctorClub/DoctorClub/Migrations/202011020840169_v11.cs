namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 200),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "UserId", "dbo.Users");
            DropIndex("dbo.Feedbacks", new[] { "UserId" });
            DropTable("dbo.Feedbacks");
        }
    }
}
