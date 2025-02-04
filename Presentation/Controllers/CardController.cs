using Application;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardServices _services;

        public CardController(ICardServices cardServices)
        {
            _services = cardServices;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCard([FromBody] CardCreateDTO dto)
        {
            var info = await _services.CreateCard(dto);
            return StatusCode(info.StatusCode, info);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PayUsingCard([FromBody] CardPayDTO dto)
        {
            var info = await _services.PayUsingCard(dto);
            return StatusCode(info.StatusCode, info);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info([FromQuery] string cardId)
        {
            var info = await _services.Info(cardId);
            return StatusCode(info.StatusCode, info);
        }
    }
}
