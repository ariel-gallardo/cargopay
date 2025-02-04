using Domain;

namespace Data
{
    public class CardRepository : BaseRepository<Card>, ICardRepository
    {
        public CardRepository(IRepository<Card> repository) : base(repository)
        {
        }
    }
}
