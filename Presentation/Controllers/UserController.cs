using Application;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _services;

        public UserController(IUserServices services)
        {
            _services = services;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            var info = await _services.LoginUser(dto);
            return StatusCode(info.StatusCode, info);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto)
        {
            var info = await _services.RegisterUser(dto);
            return StatusCode(info.StatusCode, info);
        }
    }
}
