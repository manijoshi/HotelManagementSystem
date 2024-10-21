using HotelBookingSystem.Application.DTO.UserDTO;
using HotelBookingSystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequest request)
        {
            var createdUser = await _userService.RegisterUserAsync(request);
            return Ok(new { createdUser });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {

            var token = await _userService.AuthenticateUserAsync(request);
            return Ok(new { token });

        }

    }
}
