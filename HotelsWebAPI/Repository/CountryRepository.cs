using AutoMapper;
using HotelsApplication.Data;
using HotelsApplication.Exceptions;
using HotelsApplication.Interfaces;
using HotelsApplication.Models.Country;
using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly HotelsDBContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(HotelsDBContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CountryDetailDto> GetDetails<TResult>(int id)
        {
            var country = await _context.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(c => c.Id == id);
            if (country == null)
            {
                throw new NotFoundException(nameof(Country), id);
            }
            var countryDto = _mapper.Map<CountryDetailDto>(country);
            return countryDto;
        }
    }
}
