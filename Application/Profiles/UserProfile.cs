using AutoMapper;
using Domain;

namespace Application
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {
            CreateMap<UserRegisterDTO, User>()
                .ForMember(y => y.Email, xx => xx.MapFrom(x => x.UserEmail))
                .ForMember(y => y.Password, xx => xx.MapFrom(x => x.UserPassword));

            CreateMap<UserLoginDTO, User>()
                .ForMember(y => y.Email, xx => xx.MapFrom(x => x.UserEmail))
                .ForMember(y => y.Password, xx => xx.MapFrom(x => x.UserPassword));

            CreateMap<User, UserInfoDTO>()
                .ForMember(y => y.Email, xx => xx.MapFrom(x => x.Email))
                .ForMember(y => y.Name, xx => xx.MapFrom(x => x.Name));
        }
    }
}
