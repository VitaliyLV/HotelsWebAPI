using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Data
{
    public class HotelsDBContext : DbContext
    {
        public HotelsDBContext(DbContextOptions options) : base(options)
        {

        }
    }
}
