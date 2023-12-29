using HotelsApplication.Data;
using HotelsApplication.Models.Country;

namespace HotelsApplication.Interfaces
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<CountryDetailDto> GetDetails<TResult>(int id);
    }
}

