using AutoMapper;
using HotelsApplication.Data;
using HotelsApplication.Exceptions;
using HotelsApplication.Interfaces;
using HotelsApplication.Models.Hotel;
using HotelsApplication.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;

        public HotelsController(IHotelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _repository.GetAllAsync<HotelDto>();
            return Ok(hotels);
        }

        // GET: api/Hotels/Paged?PageNumber=1&PageSize=10
        [HttpGet("Paged")]
        [Authorize]
        public async Task<ActionResult<PagedResult<HotelDto>>> GetPagedCountries([FromQuery] QueryParameters queryParams)
        {
            var pagedCountries = await _repository.GetAllAsync<HotelDto>(queryParams);
            return Ok(pagedCountries);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDetailDto>> GetHotel(int id)
        {
            var hotel = await _repository.GetDetails(id);

            if (hotel == null)
            {
                throw new NotFoundException(nameof(GetHotel), id);
            }
            var hotelDetail = _mapper.Map<HotelDetailDto>(hotel);

            return Ok(hotelDetail);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                throw new DifferentIDsException(nameof(PutHotel), id, hotelDto.Id);
            }
            var hotel = await _repository.GetAsync(id);
            if (hotel == null)
            {
                throw new NotFoundException(nameof(PutHotel), id);
            }
            _mapper.Map(hotelDto, hotel);

            try
            {
                await _repository.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    throw new NotFoundException(nameof(PutHotel), id);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto hotelDto)
        {
            var hotel = _mapper.Map<Hotel>(hotelDto);
            await _repository.AddAsync(hotel);

            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _repository.Exists(id);
        }
    }
}
