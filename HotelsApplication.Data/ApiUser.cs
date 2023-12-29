using Microsoft.AspNetCore.Identity;

namespace HotelsApplication.Data
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
    }
}
