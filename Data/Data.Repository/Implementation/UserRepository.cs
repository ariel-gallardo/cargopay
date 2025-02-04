using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IRepository<User> repository) : base(repository)
        {
        }

        public async Task<bool> UserExists(long userId)
        => await Where(x => x.Id == userId).AnyAsync();

        public async Task<bool> UserExists(string email)
        => await Where(x => x.Email == email).AnyAsync();

        public async Task<User> CreateUser(User user)
        {
            if(!await UserExists(user.Email))
            {
                if(await Insert(user) > 0) return await Where(x => x.Email == user.Email).FirstAsync();
            }
            return null;
        }
    }
}
