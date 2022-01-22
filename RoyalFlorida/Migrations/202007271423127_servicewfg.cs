namespace RoyalFlorida.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class servicewfg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "Status");
        }
    }
}
