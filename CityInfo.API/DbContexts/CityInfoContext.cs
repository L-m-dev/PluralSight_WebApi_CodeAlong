using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext 
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest  { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CityInfo.db");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "Big park"
                });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "Most visited park."
                });

            base.OnModelCreating(modelBuilder); 
        }
    }
}
