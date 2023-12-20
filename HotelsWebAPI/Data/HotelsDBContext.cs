using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Data
{
    public class HotelsDBContext : DbContext
    {
        public HotelsDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Albania", ShortName = "AL" },
                new Country { Id = 2, Name = "Argentina", ShortName = "AG" },
                new Country { Id = 3, Name = "Aruba", ShortName = "AR" }
            );
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel { Id = 1, Name = "NewSaranda", CountryId = 1, Rating = 4.3, Address = "Saranda" },
                new Hotel { Id = 2, Name = "GrandHotel", CountryId = 2, Rating = 4.6, Address = "Buenos Aires" },
                new Hotel { Id = 3, Name = "FirstHotel", CountryId = 3, Rating = 4.5, Address = "Oranjestad" },
                new Hotel { Id = 4, Name = "SeaResort", CountryId = 3, Rating = 4.7, Address = "Cura Cabai" }
                );
        }
    }
}
