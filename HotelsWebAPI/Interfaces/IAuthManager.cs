using HotelsApplication.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelsApplication.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
    }
}
