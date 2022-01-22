namespace RoyalFlorida.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class servicewfgrhas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "FirstName", c => c.String());
            AddColumn("dbo.Bookings", "LastName", c => c.String());
            AddColumn("dbo.Bookings", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Email");
            DropColumn("dbo.Bookings", "LastName");
            DropColumn("dbo.Bookings", "FirstName");
        }
    }
}
