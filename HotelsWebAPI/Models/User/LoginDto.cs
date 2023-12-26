using System.ComponentModel.DataAnnotations;

namespace HotelsApplication.Models.User
{
    public class LoginDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
