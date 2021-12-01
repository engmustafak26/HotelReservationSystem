namespace HotelReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setCancelPriceToNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reservation", "CancelPrice", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reservation", "CancelPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
