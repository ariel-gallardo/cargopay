using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Infraestructure
{
    public partial class AppSettings
    {
        public AppSettings(IConfiguration cfg)
        {
            _cfg = cfg;
            cfg.Bind(this);
            _instance = this;
        }
        private static string _jsonFileString;
        private static AppSettings _instance;
        private static IConfiguration _cfg;

        public static void Set(AppSettings cfg)
        {
            _instance = cfg;
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
