namespace HotelReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Hotel_Setting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hotels", "FreeCancelationDaysBeforeReservationDate", c => c.Int(nullable: false));
            AddColumn("dbo.Hotels", "DeductionPercentageForReservationCancelation", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Hotels", "CheckinTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Reservation", "CancelDate", c => c.DateTime());
            AddColumn("dbo.Reservation", "CancelPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Reservation", "CancelUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Reservation", "FullPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Reservation", "CancelUserId");
            AddForeignKey("dbo.Reservation", "CancelUserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Reservation", "State");
            DropColumn("dbo.Reservation", "StateChangedDate");
            DropColumn("dbo.Reservation", "RejectionReason");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservation", "RejectionReason", c => c.String(maxLength: 600));
            AddColumn("dbo.Reservation", "StateChangedDate", c => c.DateTime());
            AddColumn("dbo.Reservation", "State", c => c.Byte(nullable: false));
            DropForeignKey("dbo.Reservation", "CancelUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reservation", new[] { "CancelUserId" });
            AlterColumn("dbo.Reservation", "FullPrice", c => c.Double(nullable: false));
            DropColumn("dbo.Reservation", "CancelUserId");
            DropColumn("dbo.Reservation", "CancelPrice");
            DropColumn("dbo.Reservation", "CancelDate");
            DropColumn("dbo.Hotels", "CheckinTime");
            DropColumn("dbo.Hotels", "DeductionPercentageForReservationCancelation");
            DropColumn("dbo.Hotels", "FreeCancelationDaysBeforeReservationDate");
        }
    }
}
