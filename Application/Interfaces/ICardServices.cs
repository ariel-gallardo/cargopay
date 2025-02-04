using Domain;

namespace Application
{
    public interface ICardServices
    {
        Task<CustomResponse> CreateCard(CardCreateDTO dto);
        Task<CustomResponse> PayUsingCard(CardPayDTO dto);
        Task<CustomResponse> Info(string cardId);
    }
}
