using System.Reflection;
using Application;
using Data;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var appSettings = new AppSettings(builder.Configuration);
            builder.Services
            .AddHttpContextAccessor()
            .AddCustomAutoMapper()
            .AddJwtAuthentication()
            .AddUnitOfWork()
            .AddCustomServices()
            .AddSingleton(appSettings);

            builder.Services.AddDbContext<CargoPayContext>(o => o.UseMySQL(appSettings.ConnectionStrings.MySQL));

            builder.Services.AddCors(o =>
            {
                o.AddPolicy("AngularApp",
                policy =>
                {
                    policy.WithOrigins(appSettings.AngularUrl)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            })

            .AddPaymentFeeModule();
            builder.Services.AddControllers(o =>
            {
                o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                o.Filters.Add<ValidationFilter>();
                o.Filters.Add<Status500Filter>();
            });
            builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
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
            });

            var app = builder.Build();
            app.UseCors("AngularApp");

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
