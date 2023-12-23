using HotelsApplication.Data;

namespace HotelsApplication.Interfaces
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<Hotel> GetDetails(int id);
    }
}
