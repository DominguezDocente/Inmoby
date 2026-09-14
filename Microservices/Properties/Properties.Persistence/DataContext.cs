using Microsoft.EntityFrameworkCore;
using Properties.Domain.Entities.Locations;
using Properties.Domain.Entities.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {            
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Neighborhood> Neighborhoods { get; set; }
    }
}
