using Data;
using Domain;

namespace Application
{
    public class CardServices : ICardServices
    {
        private readonly ICardRepository _repository;

        public CardServices(ICardRepository cardRepository)
        {
            _repository = cardRepository;
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
