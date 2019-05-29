namespace AvioKompanija.Migrations
{
    using AvioKompanija.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AvioKompanija.Models.AvioKompanijaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AvioKompanija.Models.AvioKompanijaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var users = new List<User>
            {
                new User {Name="Vladimir",LastName="Jaran",Username="admin",Password="admin",Role="admin",Count=0},
                new User {Name="Stefan",LastName="Vukasinovic",Username="vuksha94",Password="vuksha94",Role="user",Count=100000}
            };
            users.ForEach(u => context.Users.AddOrUpdate(uu => uu.Username, u));
            context.SaveChanges();

            var states = new List<State>
            {
                new State { Name = "Serbia"},
                new State { Name = "Montenegro"},
                new State { Name = "Spain"},
                new State { Name = "France"},
                new State { Name = "Turkey"}
            };

            states.ForEach(s => context.States.AddOrUpdate(ss => ss.Name, s));
            context.SaveChanges();

            var cities = new List<City>
            {
                new City { Name = "Belgrade", StateId = states.Single(s => s.Name == "Serbia").Id},
                new City { Name = "Nis", StateId = states.Single(s => s.Name == "Serbia").Id},
                new City { Name = "Barcelona", StateId = states.Single(s => s.Name == "Spain").Id},
                new City { Name = "Madrid", StateId = states.Single(s => s.Name == "Spain").Id},
                new City { Name = "Podgorica", StateId = states.Single(s => s.Name == "Montenegro").Id},
                new City { Name = "Paris", StateId = states.Single(s => s.Name == "France").Id},
                new City { Name = "Istanbul", StateId = states.Single(s => s.Name == "Spain").Id},
                new City { Name = "Ankara", StateId = states.Single(s => s.Name == "Spain").Id}
            };
            cities.ForEach(c => context.Cities.AddOrUpdate(cc => cc.Name, c));
            context.SaveChanges();

            var airports = new List<Airport>
            {
                new Airport { Name = "Nikola Tesla Airport", CityId = cities.Single(c => c.Name == "Belgrade").Id},
                new Airport { Name = "Podgorica Airport", CityId = cities.Single(c => c.Name == "Podgorica").Id},
                new Airport { Name = "Airport Nis", CityId = cities.Single(c => c.Name == "Nis").Id},
                new Airport { Name = "Airport Barcelona", CityId = cities.Single(c => c.Name == "Barcelona").Id},
                new Airport { Name = "Barajas Airport", CityId = cities.Single(c => c.Name == "Madrid").Id},
                new Airport { Name = "Ataturk Airport", CityId = cities.Single(c => c.Name == "Istanbul").Id}
            };
            airports.ForEach(a => context.Airports.AddOrUpdate(aa => aa.Name, a));
            context.SaveChanges();

            var avions = new List<Avion>
            {
                new Avion { Model = "Boeing 747", Name = "Avion A01",  Capacity = 150},
                new Avion { Model = "Boeing 737", Name = "Avion A02",  Capacity = 130},
                new Avion { Model = "Boeing 787", Name = "Avion A03",  Capacity = 160}
            };
            avions.ForEach(a => context.Avions.AddOrUpdate(aa => aa.Name, a));
            context.SaveChanges();

        }
    }
}
