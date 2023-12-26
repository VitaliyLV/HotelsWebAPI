using AutoMapper;
using HotelsApplication.Data;
using HotelsApplication.Interfaces;
using HotelsApplication.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelsApplication.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _manager;

        public AuthManager(IMapper mapper, UserManager<ApiUser> manager)
        {
            _mapper = mapper;
            _manager = manager;
        }
        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = user.Email;
            var result = await _manager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                await _manager.AddToRoleAsync(user, "User");
            }
            return result.Errors;
        }
    }
}
