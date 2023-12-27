using AutoMapper;
using HotelsApplication.Data;
using HotelsApplication.Interfaces;
using HotelsApplication.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelsApplication.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _manager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        private const string _loginProvider = "HotelsAPI";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<ApiUser> manager, IConfiguration configuration)
        {
            _mapper = mapper;
            _manager = manager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            bool isValid = false;
            _user = await _manager.FindByEmailAsync(loginDto.Email);
            if (_user != null)
            {
                isValid = await _manager.CheckPasswordAsync(_user, loginDto.Password);
            }
            if (!isValid)
            {
                return null;
            }
            var token = await GenerateToken();
            return new AuthResponseDto
            {
                UserId = _user.Id,
                Token = token,
                RefreshToken = await CreateRefreshToken()
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            IdentityResult response = await CreateUser(userDto);
            if (response.Succeeded)
            {
                await _manager.AddToRoleAsync(_user, "User");
            }
            return response.Errors;
        }
        public async Task<IEnumerable<IdentityError>> RegisterAdmin(ApiUserDto userDto)
        {
            IdentityResult response = await CreateUser(userDto);
            if (response.Succeeded)
            {
                await _manager.AddToRoleAsync(_user, "Admin");
            }
            return response.Errors;
        }

        public async Task<IdentityResult> CreateUser(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = _user.Email;
            var creationResult = await _manager.CreateAsync(_user, userDto.Password);
            return creationResult;
        }

        public async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _manager.GetRolesAsync(_user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
            }.Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTSettings:Issuer"],
                audience: _configuration["JWTSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWTSettings:DurationInMins"])),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            await _manager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
            var newToken = await _manager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
            await _manager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newToken);
            return newToken;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            _user = await _manager.FindByIdAsync(request.UserId);
            if(_user == null)
            {
                return null;
            }
            var isValidRefreshToken = await _manager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);
            if (isValidRefreshToken)
            {
                var newToken = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = newToken,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }
            await _manager.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
