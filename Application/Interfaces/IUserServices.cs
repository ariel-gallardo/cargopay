using Domain;
using Microsoft.AspNetCore.Http;

namespace Application
{
    public interface IUserServices
    {
        Task<CustomResponse> LoginUser(UserLoginDTO user);
        Task<CustomResponse> RegisterUser(UserRegisterDTO user);
    }
}
