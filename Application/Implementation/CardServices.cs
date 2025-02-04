using AutoMapper;
using Data;
using Domain;

namespace Application
{
    public class CardServices : ICardServices
    {
        private readonly ICardRepository _repository;
        private readonly IMapper _mapper;

        public CardServices(ICardRepository cardRepository, IMapper mapper)
        {
            _repository = cardRepository;
            _mapper = mapper;
        }
        public async Task<CustomResponse> CreateCard(CardCreateDTO dto)
        {
            throw new NotImplementedException();
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
