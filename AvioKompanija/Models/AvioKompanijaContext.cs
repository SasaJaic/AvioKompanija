using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AvioKompanija.Models;


namespace AvioKompanija.Models
{
    public class AvioKompanijaContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public AvioKompanijaContext() : base("name=AvioKompanijaContext")
        {
        }

        public System.Data.Entity.DbSet<Avion> Avions { get; set; }

        public System.Data.Entity.DbSet<AvioKompanija.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<AvioKompanija.Models.State> States { get; set; }

        public System.Data.Entity.DbSet<AvioKompanija.Models.City> Cities { get; set; }

        public System.Data.Entity.DbSet<AvioKompanija.Models.Airport> Airports { get; set; }

        public System.Data.Entity.DbSet<AvioKompanija.Models.Flight> Flights { get; set; }

        public System.Data.Entity.DbSet<AvioKompanija.Models.Reservation> Reservations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //setting two foreign keys in Flights to 
            modelBuilder.Entity<Flight>()
                        .HasRequired<Airport>(f => f.FromAirport)
                        .WithMany(a => a.FromFlights)
                        .HasForeignKey(f => f.FromAirportId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Flight>()
                        .HasRequired<Airport>(f => f.ToAirport)
                        .WithMany(a => a.ToFlights)
                        .HasForeignKey(f => f.ToAirportId)
                        .WillCascadeOnDelete(false);
/*
            modelBuilder.Entity<Flight>()
                        .HasIndex(f => new { f.FromAirportId, f.ToAirportId })
                        .IsUnique();*/
            
            //set on delete no action
            modelBuilder.Entity<City>()
                        .HasRequired(c => c.State)
                        .WithMany(s => s.Cities)
                        .HasForeignKey(c => c.StateId)
                        .WillCascadeOnDelete(false);

           modelBuilder.Entity<Reservation>()
                        .HasIndex(r => new { r.UserId, r.FlightId })
                        .IsUnique();

        }

        
    }
}
