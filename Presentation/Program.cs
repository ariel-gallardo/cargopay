using System.Reflection;
using Application;
using Data;
using Infraestructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services
            .AddHttpContextAccessor()
            .AddCustomAutoMapper()
            .AddJwtAuthentication()
            .AddUnitOfWork()
            .AddCustomServices()
            .AddSingleton<AppSettings>()
            .AddDbContext<CargoPayContext>(o => o.UseNpgsql(AppSettings.Config.ConnectionStrings.PostgreSQL))
            .AddPaymentFeeModule();
            builder.Services.AddControllers(o =>
            {
                o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
            builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(o =>
            {
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor ingrese el token en formato 'Bearer {token}'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var ctrlOutput = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                o.IncludeXmlComments(ctrlOutput, true);
                o.EnableAnnotations();
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
