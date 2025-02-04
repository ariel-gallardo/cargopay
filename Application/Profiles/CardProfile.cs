using AutoMapper;
using Domain;
using Infraestructure;

namespace Application
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<CardCreateDTO, Card>()
                .ForMember(x => x.UserId, xx => xx.MapFrom(y => y.UserId))
                .ForMember(x => x.Balance, xx => xx.MapFrom(y => y.Balance));

            CreateMap<CardPayDTO, Card>()
                .ForMember(x => x.Id, xx => xx.MapFrom(y => y.CardId))
                .ForMember(x => x.Balance, xx => xx.MapFrom((src, dest) =>
                {
                    return src.Amount * -1 + dest.Balance;
                }));

            CreateMap<Card, CardInfoDTO>()
                .ForMember(x => x.CardId, xx => xx.MapFrom(y => y.Id))
                .ForMember(x => x.UserId, xx => xx.MapFrom(y => y.UserId))
                .ForMember(x => x.Balance, xx => xx.MapFrom(y => y.Balance));

            CreateMap<Card, Card>()
                .ForMember(x => x.UserId, xx => xx.MapFrom((src, dest) => dest.UserId != src.UserId && src.UserId > 0 ? src.UserId : dest.UserId))
                .ForMember(x => x.Balance, xx => xx.MapFrom((src, dest) => dest.Balance != src.Balance && src.Balance >= 0 ? src.Balance : dest.Balance))
                .ForMember(x => x.CreatedAt, xx => xx.MapFrom((src, dest) => dest.CreatedAt != src.CreatedAt ? src.CreatedAt : dest.CreatedAt))
                .ForMember(x => x.UpdatedAt, xx => xx.MapFrom((src, dest) => dest.UpdatedAt != src.UpdatedAt ? src.UpdatedAt : dest.UpdatedAt))
                .ForMember(x => x.DeletedAt, xx => xx.MapFrom((src, dest) => dest.DeletedAt != src.DeletedAt ? src.DeletedAt : dest.DeletedAt));
        }
    }
}
