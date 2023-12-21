using HotelsApplication.Data;
using HotelsApplication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelsDBContext _context;

        public CountryRepository(HotelsDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
