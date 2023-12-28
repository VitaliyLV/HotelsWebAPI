using AutoMapper;
using HotelsApplication.Data;
using HotelsApplication.Exceptions;
using HotelsApplication.Interfaces;
using HotelsApplication.Models.Country;
using HotelsApplication.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelsApplication.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CountriesV2Controller : ControllerBase
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public CountriesV2Controller(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
            var countries = await _repository.GetAllAsync<CountryDto>();
            return Ok(countries);
        }

        // GET: api/Countries/Paged?PageNumber=1&PageSize=10
        [HttpGet("Paged")]
        [Authorize]
        public async Task<ActionResult<PagedResult<CountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParams)
        {
            var pagedCountries = await _repository.GetAllAsync<CountryDto>(queryParams);
            return Ok(pagedCountries);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CountryDetailDto>> GetCountry(int id)
        {
            var country = await _repository.GetDetails(id);

            if (country == null)
            {
                throw new NotFoundException(nameof(GetCountry), id);
            }
            var countryDto = _mapper.Map<CountryDetailDto>(country);

            return Ok(countryDto);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutCountry(int id, CountryDto countryDto)
        {
            if (id != countryDto.Id)
            {
                throw new DifferentIDsException(nameof(PutCountry), id, countryDto.Id);
            }
            var country = await _repository.GetAsync(id);
            if (country == null)
            {
                throw new NotFoundException(nameof(PutCountry), id);
            }
            _mapper.Map(countryDto, country);

            try
            {
                await _repository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    throw new NotFoundException(nameof(PutCountry), id);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CountryDto>> PostCountry(CreateCountryDto createCountry)
        {
            var country = _mapper.Map<Country>(createCountry);
            var newCountry = await _repository.AddAsync(country);
            var returnCountry = _mapper.Map<CountryDto>(newCountry);

            return CreatedAtAction("GetCountry", new { id = returnCountry.Id }, returnCountry);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _repository.GetAsync(id);
            if (country == null)
            {
                throw new NotFoundException(nameof(DeleteCountry), id);
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _repository.Exists(id);
        }
    }
}
