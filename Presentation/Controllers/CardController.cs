using Application;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Módulo de Tarjetas")]
    public class CardController : ControllerBase
    {
        private readonly ICardServices _services;

        public CardController(ICardServices cardServices)
        {
            _services = cardServices;
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "Crear", Description = "Crear una tarjeta con saldo y asociarla a un usuario.")]
        public async Task<IActionResult> CreateCard([FromBody, SwaggerRequestBody(Description = "Id del usuario y balance.")] CardCreateDTO dto)
        {
            var info = await _services.CreateCard(dto);
            return StatusCode(info.StatusCode, info);
        }

        [HttpPut]
        [Authorize]
        [SwaggerOperation(Summary = "Pagar", Description = "Pagar con tarjeta existente.")]
        public async Task<IActionResult> PayUsingCard([FromBody, SwaggerRequestBody(Description = "Monto a pagar y id del usuario.")] CardPayDTO dto)
        {
            var info = await _services.PayUsingCard(dto);
            return StatusCode(info.StatusCode, info);
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation(Summary = "Informacion", Description = "Informacion de tarjeta.")]
        public async Task<IActionResult> Info([FromQuery, SwaggerParameter(Description = "Id de la tarjeta.")] string cardId)
        {
            var info = await _services.Info(cardId);
            return StatusCode(info.StatusCode, info);
        }
    }
}
