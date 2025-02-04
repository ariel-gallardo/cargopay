using System.Text;
using Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application
{
    public static class JWTExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = AppSettings.Config.JWT.ValidateIssuer,
                        ValidateAudience = AppSettings.Config.JWT.ValidateAudience,
                        ValidateLifetime = AppSettings.Config.JWT.ValidateLifetime,
                        ValidIssuer = AppSettings.Config.JWT.Issuer,
                        ValidAudience = AppSettings.Config.JWT.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.Config.JWT.SecretKey))
                    };
                });
            return services;
        }
    }
}
