using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Infraestructure
{
    public partial class AppSettings
    {
        public AppSettings()
        {
            
        }
        private static AppSettings _instance;
        public void Set(AppSettings s){
            _instance = s;
        }

        public static AppSettings Config
        {
            get => _instance;
        }
        public JwtEntity JWT { get; set; }
        public PaymentFeeEntity PaymentFee { get; set; }
        public int Take { get; set; }
    }
}
