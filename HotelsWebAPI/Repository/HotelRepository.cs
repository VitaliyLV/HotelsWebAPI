using AutoMapper;
using HotelsApplication.Data;
using HotelsApplication.Exceptions;
using HotelsApplication.Interfaces;
using HotelsApplication.Models.Hotel;
using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Repository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        HotelsDBContext _context;
        private readonly IMapper _mapper;

        public HotelRepository(HotelsDBContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<HotelDetailDto> GetDetails<HotelDetailDto>(int id)
        {
            var hotel = await _context.Hotels.Include(c => c.Country).FirstOrDefaultAsync(c => c.Id == id);
            if (hotel == null)
            {
                throw new NotFoundException(nameof(Hotel), id);
            }
            var hotelDetail = _mapper.Map<HotelDetailDto>(hotel);
            return hotelDetail;
        }
    }
}
