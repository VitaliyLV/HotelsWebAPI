using HotelsApplication.Models.Hotel;

namespace HotelsApplication.Models.Country
{
    public class CountryDetailDto : BaseCountryDto
    {
        public int Id { get; set; }
        public List<HotelDto> Hotels { get; set; }
    }
}
