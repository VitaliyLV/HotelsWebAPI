using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelsApplication.Data.Configurations
{
    public class HotelConfig : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel { Id = 1, Name = "NewSaranda", CountryId = 1, Rating = 4.3, Address = "Saranda" },
                new Hotel { Id = 2, Name = "GrandHotel", CountryId = 2, Rating = 4.6, Address = "Buenos Aires" },
                new Hotel { Id = 3, Name = "FirstHotel", CountryId = 3, Rating = 4.5, Address = "Oranjestad" },
                new Hotel { Id = 4, Name = "SeaResort", CountryId = 3, Rating = 4.7, Address = "Cura Cabai" }
                );
        }
    }
}
