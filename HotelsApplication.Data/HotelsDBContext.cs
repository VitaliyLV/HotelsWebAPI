using HotelsApplication.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HotelsApplication.Data
{
    public class HotelsDBContext : IdentityDbContext<ApiUser>
    {
        public HotelsDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new HotelConfig());
        }
    }
    public class HotelsDBContextFactory : IDesignTimeDbContextFactory<HotelsDBContext>
    {
        public HotelsDBContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            var optBuilder = new DbContextOptionsBuilder<HotelsDBContext>();
            var connection = config.GetConnectionString("HotelsDBConnectionString");
            optBuilder.UseSqlServer(connection);
            return new HotelsDBContext(optBuilder.Options);
        }
    }
}
