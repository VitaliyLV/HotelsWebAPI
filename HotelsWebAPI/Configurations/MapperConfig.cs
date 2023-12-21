using AutoMapper;
using HotelsApplication.Data;
using HotelsApplication.Models.Country;
using HotelsApplication.Models.Hotel;

namespace HotelsApplication.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CountryDetailDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }
}
