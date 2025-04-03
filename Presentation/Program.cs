using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Application;
using Data;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #if DEBUG
                System.Diagnostics.Debugger.Launch();
            #endif
            
            var builder = WebApplication.CreateBuilder(args);
            var appSettings = new AppSettings();
            builder.Configuration.Bind("AppSettings", appSettings);
            appSettings.Set(appSettings);
            var services = builder.Services
            .AddHttpContextAccessor()
            .AddCustomAutoMapper()
            .AddSingleton(appSettings);

            services.AddJwtAuthentication()
            .AddUnitOfWork()
            .AddCustomServices();

            builder.WebHost.ConfigureKestrel(options =>
            {
                var store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                var cert = store.Certificates
                    .Find(X509FindType.FindByIssuerName, "CargoPayAPI", false)
                    .OfType<X509Certificate2>()
                    .FirstOrDefault();
                options.Listen(IPAddress.Any,443,o => {
                    o.UseHttps(o2 => {
                        o2.ServerCertificate = cert;
                    });
                });
            });

            services.AddDbContext<CargoPayContext>(o => o.UseNpgsql(builder.Configuration["AZURE_POSTGRESQL_CONNECTIONSTRING"]))
            .AddPaymentFeeModule();
            builder.Services.AddControllers(o =>
            {
                o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                o.Filters.Add<Status500Filter>();
                o.Filters.Add<ValidationFilter>();
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
                o.EnableAnnotations();
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CargoPayContext>();
                dbContext.Database.Migrate();
            }
            app.Run();
        }
    }
}
