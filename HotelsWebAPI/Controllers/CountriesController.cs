using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelsApplication.Data;
using HotelsApplication.Models.Country;
using AutoMapper;
using HotelsApplication.Repository;
using HotelsApplication.Interfaces;

namespace HotelsApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public CountriesController(ICountryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries()
        {
            var countries = await _repository.GetAllAsync();
            var getCountries = _mapper.Map<List<CountryDto>>(countries);
            return Ok(getCountries);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDetailDto>> GetCountry(int id)
        {
            var country = await _repository.GetDetails(id);

            if (country == null)
            {
                return NotFound();
            }
            var countryDto = _mapper.Map<CountryDetailDto>(country);

            return Ok(countryDto);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, CountryDto countryDto)
        {
            if (id != countryDto.Id)
            {
                return BadRequest();
            }
            var country = await _repository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
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
                    return NotFound();
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
        public async Task<ActionResult<CountryDto>> PostCountry(CreateCountryDto createCountry)
        {
            var country = _mapper.Map<Country>(createCountry);
            var newCountry = await _repository.AddAsync(country);
            var returnCountry = _mapper.Map<CountryDto>(newCountry);

            return CreatedAtAction("GetCountry", new { id = returnCountry.Id }, returnCountry);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _repository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
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
