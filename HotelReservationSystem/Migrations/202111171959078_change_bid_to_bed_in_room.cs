namespace HotelReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_bid_to_bed_in_room : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "BedsCount", c => c.Int(nullable: false));
            DropColumn("dbo.Rooms", "BidsCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rooms", "BidsCount", c => c.Int(nullable: false));
            DropColumn("dbo.Rooms", "BedsCount");
        }
    }
}
