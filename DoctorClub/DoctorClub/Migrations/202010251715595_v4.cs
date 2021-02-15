namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "AccStatus", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "online", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "ActivePoints", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "ActivePoints", c => c.Int());
            AlterColumn("dbo.Users", "online", c => c.Boolean());
            AlterColumn("dbo.Users", "AccStatus", c => c.Int());
        }
    }
}
