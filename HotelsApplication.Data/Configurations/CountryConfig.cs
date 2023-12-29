using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace HotelsApplication.Data.Configurations
{
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country { Id = 1, Name = "Albania", ShortName = "AL" },
                new Country { Id = 2, Name = "Argentina", ShortName = "AG" },
                new Country { Id = 3, Name = "Aruba", ShortName = "AR" }
           );
        }
    }
}
