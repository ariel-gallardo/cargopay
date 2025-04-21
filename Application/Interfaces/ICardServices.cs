using Domain;

namespace Application
{
    public interface ICardServices
    {
        Task<CustomResponse> CreateCard(CardCreateDTO dto);
        Task<CustomResponse> PayUsingCard(CardPayDTO dto);
        Task<CustomResponse> Info(string cardId);
        Task<CustomResponse> GetCardsForCurrentUser(int page, int take, string orderBy, string searchBy);
    }
}
