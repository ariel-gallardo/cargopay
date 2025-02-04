using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class CustomAutoMapperExtensions
    {
        public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services) {

            services.AddAutoMapper(x =>
            {
                x.AddProfile<UserProfile>();
            });
            return services;
        }
    }
}
