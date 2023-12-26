using System.ComponentModel.DataAnnotations;

namespace HotelsApplication.Models.User
{
    public class ApiUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Company { get; set; }

    }
}
