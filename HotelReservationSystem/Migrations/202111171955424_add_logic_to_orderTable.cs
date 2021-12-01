namespace HotelReservationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_logic_to_orderTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Orders", newName: "Reservation");
            RenameColumn(table: "dbo.Reservation", name: "Hotel_Id", newName: "HotelId");
            RenameColumn(table: "dbo.Reservation", name: "Customer_Id", newName: "HotelCustomerId");
            RenameIndex(table: "dbo.Reservation", name: "IX_Customer_Id", newName: "IX_HotelCustomerId");
            RenameIndex(table: "dbo.Reservation", name: "IX_Hotel_Id", newName: "IX_HotelId");
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(maxLength: 650),
                        BidsCount = c.Int(nullable: false),
                        PricePerNight = c.Double(nullable: false),
                        IsInactive = c.Boolean(nullable: false),
                        HotelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Hotels", t => t.HotelId, cascadeDelete: false)
                .Index(t => t.HotelId);
            
            AddColumn("dbo.Customers", "Email", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Customers", "IsInactive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Hotels", "IsInactive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Hotels", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Reservation", "State", c => c.Byte(nullable: false));
            AddColumn("dbo.Reservation", "StateChangedDate", c => c.DateTime());
            AddColumn("dbo.Reservation", "RejectionReason", c => c.String(maxLength: 600));
            AddColumn("dbo.Reservation", "RoomId", c => c.Int(nullable: false));
            AddColumn("dbo.Reservation", "RequesterUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Reservation", "UpdatedByHotelAdminUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 250));
            CreateIndex("dbo.Customers", "UserId");
            CreateIndex("dbo.Hotels", "UserId");
            CreateIndex("dbo.Reservation", "RoomId");
            CreateIndex("dbo.Reservation", "RequesterUserId");
            CreateIndex("dbo.Reservation", "UpdatedByHotelAdminUserId");
            AddForeignKey("dbo.Customers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Hotels", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Reservation", "RequesterUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Reservation", "RoomId", "dbo.Rooms", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Reservation", "UpdatedByHotelAdminUserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Hotels", "PricePerNight");
            DropColumn("dbo.Hotels", "IsAllInclusive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Hotels", "IsAllInclusive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Hotels", "PricePerNight", c => c.Double(nullable: false));
            DropForeignKey("dbo.Reservation", "UpdatedByHotelAdminUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reservation", "RoomId", "dbo.Rooms");
            DropForeignKey("dbo.Rooms", "HotelId", "dbo.Hotels");
            DropForeignKey("dbo.Reservation", "RequesterUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Hotels", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Rooms", new[] { "HotelId" });
            DropIndex("dbo.Reservation", new[] { "UpdatedByHotelAdminUserId" });
            DropIndex("dbo.Reservation", new[] { "RequesterUserId" });
            DropIndex("dbo.Reservation", new[] { "RoomId" });
            DropIndex("dbo.Hotels", new[] { "UserId" });
            DropIndex("dbo.Customers", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.Reservation", "UpdatedByHotelAdminUserId");
            DropColumn("dbo.Reservation", "RequesterUserId");
            DropColumn("dbo.Reservation", "RoomId");
            DropColumn("dbo.Reservation", "RejectionReason");
            DropColumn("dbo.Reservation", "StateChangedDate");
            DropColumn("dbo.Reservation", "State");
            DropColumn("dbo.Hotels", "UserId");
            DropColumn("dbo.Hotels", "IsInactive");
            DropColumn("dbo.Customers", "UserId");
            DropColumn("dbo.Customers", "IsInactive");
            DropColumn("dbo.Customers", "Email");
            DropTable("dbo.Rooms");
            RenameIndex(table: "dbo.Reservation", name: "IX_HotelId", newName: "IX_Hotel_Id");
            RenameIndex(table: "dbo.Reservation", name: "IX_HotelCustomerId", newName: "IX_Customer_Id");
            RenameColumn(table: "dbo.Reservation", name: "HotelCustomerId", newName: "Customer_Id");
            RenameColumn(table: "dbo.Reservation", name: "HotelId", newName: "Hotel_Id");
            RenameTable(name: "dbo.Reservation", newName: "Orders");
        }
    }
}
