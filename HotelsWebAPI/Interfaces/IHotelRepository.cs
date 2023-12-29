using HotelsApplication.Data;

namespace HotelsApplication.Interfaces
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<HotelDetailDto> GetDetails<HotelDetailDto>(int id);
    }
}
