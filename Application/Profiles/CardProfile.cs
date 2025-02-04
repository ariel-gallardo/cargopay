using AutoMapper;
using Domain;

namespace Application
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardCreateDTO, Card>()
                .ForMember(x => x.UserId, xx => xx.MapFrom(y => y.UserId))
                .ForMember(x => x.Balance, xx => xx.MapFrom(y => y.Balance));
        }
    }
}
