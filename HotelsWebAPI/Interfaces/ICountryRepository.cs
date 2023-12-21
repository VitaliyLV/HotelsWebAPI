using HotelsApplication.Data;

namespace HotelsApplication.Interfaces
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }
}

