namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Feedbacks", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Feedbacks", "Content", c => c.String());
        }
    }
}
