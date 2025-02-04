using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class PaymentFeeExtensions
    {
        public static IServiceCollection AddPaymentFeeModule(this IServiceCollection services)
        {
            services.AddSingleton<IPaymentFeesServices, PaymentFeesServices>();
            return services;
        }
    }
}
