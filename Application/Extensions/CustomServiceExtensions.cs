using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class CustomServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordServices, PasswordServices>();
            services.AddScoped<IUserServices, UserServices>();
            return services;
        }
    }
}
