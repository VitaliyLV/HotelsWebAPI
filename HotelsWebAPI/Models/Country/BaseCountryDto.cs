using System.ComponentModel.DataAnnotations;

namespace HotelsApplication.Models.Country
{
    public class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
