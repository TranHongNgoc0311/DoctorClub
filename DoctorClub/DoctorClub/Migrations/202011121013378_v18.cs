namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v18 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserNotifications", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserNotifications", "Status", c => c.Boolean());
        }
    }
}
