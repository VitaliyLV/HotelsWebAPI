using HotelsApplication.Data;
using HotelsApplication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Repository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        HotelsDBContext _context;
        public HotelRepository(HotelsDBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Hotel> GetDetails(int id)
        {
            return await _context.Hotels.Include(c => c.Country).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
