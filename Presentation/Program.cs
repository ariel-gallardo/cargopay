using Application;
using Data;
using Infraestructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
            .AddDbContext<CargoPayContext>(o => o.UseSqlServer(AppSettings.Config.ConnectionStrings.MSSQL));
            builder.Services.AddControllers(o =>
            {
                o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
            builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
