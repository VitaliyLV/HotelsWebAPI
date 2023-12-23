using HotelsApplication.Models.Country;

namespace HotelsApplication.Models.Hotel
{
    public class HotelDetailDto : BaseHotelDto
    {
        public int Id { get; set; }
        public CountryDto Country { get; set; }
    }
}
