using System.ComponentModel.DataAnnotations;

namespace HotelsApplication.Models.Hotel
{
    public abstract class BaseHotelDto
    {
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
