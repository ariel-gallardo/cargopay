using AutoMapper;
using Domain;
using Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CardRepository : BaseRepository<Card>, ICardRepository
    {
        private readonly IMapper _mapper;

        public CardRepository(IRepository<Card> repository, IMapper mapper) : base(repository)
        {
            _mapper = mapper;
        }

        public async Task<Pagination<CardInfoDTO>> CardsByUser(long userId, int page, int take, string orderBy, string searchBy)
        => await Where(x => x.UserId == userId)
            .Include(x => x.User)
            .AddSearchByFilters(searchBy)
            .OrderBy(orderBy.OrderByExpressionMaker<Card>())
            .AsPaginate<Card,CardInfoDTO>(_mapper,page, take);
    }
}
