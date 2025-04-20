using Domain;

namespace Data
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Pagination<CardInfoDTO>> CardsByUser(long userId, int page, int take, string orderBy);
    }
}
