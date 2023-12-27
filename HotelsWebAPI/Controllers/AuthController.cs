using HotelsApplication.Interfaces;
using HotelsApplication.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelsApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _manager;

        public AuthController(IAuthManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Route("register")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register(ApiUserDto user)
        {
            var errors = await _manager.Register(user);
            if(errors.Any())
            {
                foreach(var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPost]
        [Route("registerAdmin")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RegisterAdmin(ApiUserDto user)
        {
            var errors = await _manager.RegisterAdmin(user);
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var authResponse = await _manager.Login(loginDto);
            if (authResponse is null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);
        }
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken(AuthResponseDto request)
        {
            var authResponse = await _manager.VerifyRefreshToken(request);
            if (authResponse is null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);
        }
    }
}
