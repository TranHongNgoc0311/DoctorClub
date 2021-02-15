namespace DoctorClub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Level", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Remember_token", c => c.String());
            AddColumn("dbo.Users", "token_created_at", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "token_created_at");
            DropColumn("dbo.Users", "Remember_token");
            DropColumn("dbo.Users", "Level");
        }
    }
}
