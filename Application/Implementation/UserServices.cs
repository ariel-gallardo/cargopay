using System.Security.Claims;
using AutoMapper;
using Data;
using Domain;
using Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordServices _pwdServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public UserServices(IUserRepository repository, IPasswordServices pwdServices, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _repository = repository;
            _pwdServices = pwdServices;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        public IEnumerable<Claim> CurrentUserClaims { get => _httpContext?.HttpContext?.User?.Claims ?? new Claim[] {}; }

        public User CurrentUser { get
            {
                User result = null;
                if(CurrentUserClaims.Count() > 0)
                {
                    var cId = long.Parse(CurrentUserClaims.First(x => ClaimTypes.Sid == x.Type).Value);
                    result = _repository.Where(x => x.Id == cId).Include(x => x.Cards).First();
                }
                return result;
            }
        }

        public async Task<CustomResponse> LoginUser(UserLoginDTO user)
        {
            var result = new CustomResponse();
            if (await _repository.UserExists(user.UserEmail))
            {
                var cUser = await _repository.Where(x => x.Email == user.UserEmail).FirstOrDefaultAsync();
                if(cUser != null)
                {
                    if(_pwdServices.Ok(user.UserPassword, cUser.Password))
                    {
                        var (token,expTime) = _pwdServices.GenerateToken(cUser);
                        _httpContext.HttpContext.Response.Headers.Add("Authorization", token);
                        result.Data = _mapper.Map<User,UserInfoDTO>(cUser);
                        result.Data.ExpirationTime = expTime;
                        result.Message = Messages.Exists(Entities.User, user.UserEmail);
                        result.StatusCode = StatusCodes.Status200OK;
                    }
                    else
                    {
                        result.Message = Messages.InvalidRequest(Entities.User, "PASSWORD");
                        result.StatusCode = StatusCodes.Status400BadRequest;
                    }
                }
                else
                {
                    result.Message = Messages.NotExists(Entities.User, user.UserEmail);
                    result.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                result.Message = Messages.InvalidRequest(Entities.User);
                result.StatusCode = StatusCodes.Status401Unauthorized;
            }
            return result;
        }

        public async Task<CustomResponse> RegisterUser(UserRegisterDTO user)
        {
            var result = new CustomResponse();
            if (!await _repository.UserExists(user.UserEmail))
            {
                user.UserPassword = _pwdServices.Encrypt(user.UserPassword);
                var newUser = _mapper.Map<UserRegisterDTO, User>(user);
                newUser = await _repository.CreateUser(newUser);
                if (newUser != null) {
                    result.Message = Messages.Created(Entities.User, user.UserEmail);
                    result.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    result.Message = Messages.NotFound(Entities.User, user.UserEmail);
                    result.StatusCode = StatusCodes.Status404NotFound;
                }

            }
            else
            {
                result.Message = Messages.AlreadyExists(Entities.User, user.UserEmail);
                result.StatusCode = StatusCodes.Status400BadRequest;
            }
            return result;
        }
    }
}
