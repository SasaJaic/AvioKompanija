namespace AvioKompanija.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05020229 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Airports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32),
                        StateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Flights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        time = c.DateTime(nullable: false),
                        TicketsLeft = c.Int(nullable: false),
                        FromAirportId = c.Int(nullable: false),
                        ToAirportId = c.Int(nullable: false),
                        AvionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Avions", t => t.AvionId, cascadeDelete: true)
                .ForeignKey("dbo.Airports", t => t.FromAirportId)
                .ForeignKey("dbo.Airports", t => t.ToAirportId)
                .Index(t => t.FromAirportId)
                .Index(t => t.ToAirportId)
                .Index(t => t.AvionId);
            
            CreateTable(
                "dbo.Avions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Model = c.String(nullable: false),
                        Capacity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfTickets = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        FlightId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flights", t => t.FlightId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.UserId, t.FlightId }, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Role = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Flights", "ToAirportId", "dbo.Airports");
            DropForeignKey("dbo.Reservations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reservations", "FlightId", "dbo.Flights");
            DropForeignKey("dbo.Flights", "FromAirportId", "dbo.Airports");
            DropForeignKey("dbo.Flights", "AvionId", "dbo.Avions");
            DropForeignKey("dbo.Cities", "StateId", "dbo.States");
            DropForeignKey("dbo.Airports", "CityId", "dbo.Cities");
            DropIndex("dbo.Reservations", new[] { "UserId", "FlightId" });
            DropIndex("dbo.Flights", new[] { "AvionId" });
            DropIndex("dbo.Flights", new[] { "ToAirportId" });
            DropIndex("dbo.Flights", new[] { "FromAirportId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropIndex("dbo.Airports", new[] { "CityId" });
            DropTable("dbo.Users");
            DropTable("dbo.Reservations");
            DropTable("dbo.Avions");
            DropTable("dbo.Flights");
            DropTable("dbo.States");
            DropTable("dbo.Cities");
            DropTable("dbo.Airports");
        }
    }
}
