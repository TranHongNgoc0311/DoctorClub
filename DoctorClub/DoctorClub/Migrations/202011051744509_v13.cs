namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserSpecializations", "To", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserSpecializations", "To", c => c.Int(nullable: false));
        }
    }
}
