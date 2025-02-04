using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection s)
        {
            #region User
            s.AddScoped<IRepository<User>, BigIntRepository<User>>();
            s.AddScoped<IUserRepository, UserRepository>();
            #endregion

            s.AddScoped<IUnitOfWork, UnitOfWork>();
            return s;
        }
    }
}
