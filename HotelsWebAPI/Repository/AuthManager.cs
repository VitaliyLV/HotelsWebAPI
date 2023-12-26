using AutoMapper;
using HotelsApplication.Data;
using HotelsApplication.Interfaces;
using HotelsApplication.Models.User;
using Microsoft.AspNetCore.Identity;
using Serilog;

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

        public async Task<bool> Login(LoginDto loginDto)
        {
            bool isValid = false;
            try
            {
                var user = await _manager.FindByEmailAsync(loginDto.Email);
                if (user != null)
                {
                    isValid = await _manager.CheckPasswordAsync(user, loginDto.Password);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during login.");
            }
            return isValid;
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
