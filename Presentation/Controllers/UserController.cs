using System.Net.Mime;
using Application;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Módulo de Usuario")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _services;

        public UserController(IUserServices services)
        {
            _services = services;
        }
        [HttpPost("login")]
        [SwaggerResponse(200, "Login exitoso.", typeof(CustomResponse<UserInfoDTO>), "application/json")]
        [SwaggerResponse(400, "Login fallido.", typeof(CustomResponse), "application/json")]
        [SwaggerResponse(404, "No existe.", typeof(CustomResponse), "application/json")]
        [SwaggerOperation(Summary = "Iniciar Sesion", Description = "Iniciar Sesion | Usuarios no autenticados.")]
        public async Task<IActionResult> Login([FromBody, SwaggerRequestBody(Description = "Credenciales del usuario (usuario y contraseña)")] UserLoginDTO dto)
        {
            var info = await _services.LoginUser(dto);
            return StatusCode(info.StatusCode, info);
        }

        [HttpPost("register")]
        [SwaggerResponse(200, "Registro exitoso.", typeof(CustomResponse<UserInfoDTO>), "application/json")]
        [SwaggerResponse(400, "Registro fallido.", typeof(CustomResponse), "application/json")]
        [SwaggerOperation(Summary = "Registrarse", Description = "Registrarse como usuario")]
        public async Task<IActionResult> Register([FromBody, SwaggerRequestBody(Description = "Credenciales del usuario (usuario y contraseña)")] UserRegisterDTO dto)
        {
            var info = await _services.RegisterUser(dto);
            return StatusCode(info.StatusCode, info);
        }
    }
}
