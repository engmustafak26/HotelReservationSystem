namespace HotelReservationSystem.Migrations
{
    using HotelReservationSystem.Models;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers1 : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                
          
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'56999692-99d2-4d6d-b661-c889ca08f2e0', N'CanManageHotels')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'59999692-99d2-4d6d-b661-c889ca08f2e0', N'Customer')
                
            ");
        }
        
        public override void Down()
        {
        }
    }
}
