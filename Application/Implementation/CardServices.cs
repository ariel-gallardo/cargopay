using System.Security.Claims;
using AutoMapper;
using Data;
using Domain;
using Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class CardServices : ICardServices
    {
        private readonly ICardRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;

        public CardServices(IUnitOfWork unitOfWork, IMapper mapper, IUserServices userServices)
        {
            _repository = unitOfWork.Card;
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
            var result = new CustomResponse();
            if(_userServices.CurrentUser.Cards.Any(x => x.Id == dto.CardId))
            {
                var cardDb = await _repository.Where(x => x.Id == dto.CardId).FirstAsync();
                try
                {
                    var opMap = _mapper.Map<CardPayDTO, Card>(dto,cardDb);
                    if (opMap.Balance < 0 || dto.Amount == 0) throw new SubtractionException("PAY_AMOUNT", dto.Amount * -1 + cardDb.Balance);
                    else if(dto.Amount != 0)
                    {
                        cardDb.Balance = opMap.Balance;
                        await _repository.Update(cardDb);
                        result.Data = _mapper.Map<Card, CardInfoDTO>(opMap);
                        result.Message = Messages.Updated(Entities.Card, dto.CardId);
                        result.StatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        result.Data = _mapper.Map<Card, CardInfoDTO>(opMap);
                        result.Message = Messages.NotUpdated(Entities.Card, dto.CardId);
                        result.StatusCode = StatusCodes.Status400BadRequest;
                    }

                }
                catch (SubtractionException sE)
                {
                    result.Data = cardDb;
                    result.Message = sE.Message;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                result.Message = Messages.NotAssociated(Entities.Card, dto.CardId);
                result.StatusCode = StatusCodes.Status400BadRequest;
            }
            return result;
        }

        public async Task<CustomResponse> Info(string cardId)
        {
            var result = new CustomResponse();
            if (_userServices.CurrentUser.Cards.Any(x => x.Id == cardId))
            {
                result.Data = _mapper.Map<Card,CardInfoDTO>(_userServices.CurrentUser.Cards.First(x => x.Id == cardId));
                result.Message = Messages.Exists(Entities.Card, cardId);
                result.StatusCode = StatusCodes.Status200OK;
            }
            else
            {
                result.StatusCode = StatusCodes.Status403Forbidden;
                result.Message = Messages.NotAssociated(Entities.Card, cardId);
            }
            return result;
        }
    }
}
