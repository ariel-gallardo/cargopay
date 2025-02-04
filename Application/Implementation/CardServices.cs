using System.Security.Claims;
using AutoMapper;
using Data;
using Domain;
using Infraestructure;
using Microsoft.AspNetCore.Http;

namespace Application
{
    public class CardServices : ICardServices
    {
        private readonly ICardRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;

        public CardServices(ICardRepository cardRepository, IMapper mapper, IUserServices userServices)
        {
            _repository = cardRepository;
            _mapper = mapper;
            _userServices = userServices;
        }
        public async Task<CustomResponse> CreateCard(CardCreateDTO dto)
        {
            var cardEntity = _mapper.Map<CardCreateDTO, Card>(dto);
            if (cardEntity.UserId == 0) cardEntity.UserId = long.Parse(_userServices.CurrentUserClaims.First(x => ClaimTypes.Sid == x.Type).Value);
            var carOpCreate = await _repository.Insert(cardEntity);
            return new CustomResponse
            {
                Data = carOpCreate > 0 ? _mapper.Map<Card,CardInfoDTO>(cardEntity) : null,
                StatusCode = carOpCreate > 0 ? StatusCodes.Status201Created : StatusCodes.Status500InternalServerError,
                Message = carOpCreate > 0 ? Messages.Created(Entities.Card, cardEntity.Id) : Messages.CannotBeCreated(Entities.Card,"CHECK")
            };
        }

        public async Task<CustomResponse> PayUsingCard(CardPayDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomResponse> Info(string cardId)
        {
            throw new NotImplementedException();
        }
    }
}
