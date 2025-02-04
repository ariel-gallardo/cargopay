using Domain;

namespace Data
{
    public interface IUserRepository : IRepository<User>
    {
        IUnitOfWork UnitOfWork { get; set; }
        Task<bool> UserExists(long userId);
        Task<bool> UserExists(string email);
        Task<User> CreateUser(User user);
    }
}
