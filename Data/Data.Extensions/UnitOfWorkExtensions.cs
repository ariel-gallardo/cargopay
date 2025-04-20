using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection s)
        {
            #region User
            s.AddScoped<IRepository<User>, Repository<User>>();
            s.AddScoped<IRepository<Card>, Repository<Card>>();
            s.AddScoped<IUserRepository, UserRepository>();
            s.AddScoped<ICardRepository, CardRepository>();
            #endregion

            s.AddScoped<IUnitOfWork, UnitOfWork>();
            return s;
        }
    }
}
