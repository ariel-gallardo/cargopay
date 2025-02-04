using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Infraestructure
{
    public partial class AppSettings
    {
        public AppSettings()
        {
            
        }
        public AppSettings(IConfiguration cfg)
        {
            _cfg = cfg;
            cfg.Bind(this);
            _instance = this;
        }
        private static string _jsonFileString;
        private static AppSettings _instance;
        private static IConfiguration _cfg;

        public static AppSettings Config
        {
            get
            {
                if (string.IsNullOrEmpty(_jsonFileString) && _cfg == null)
                {
                    if(_cfg == null)
                        _jsonFileString = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"));
                    _instance = JsonSerializer.Deserialize<AppSettings>(_jsonFileString, new JsonSerializerOptions
                    {
                       DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                       IgnoreReadOnlyProperties = true,
                       IgnoreReadOnlyFields = true,
                       PropertyNameCaseInsensitive = true
                    });
                }
                return _instance;
            }
        }
        public JwtEntity JWT { get; set; }
        public ConnectionStringsEntity ConnectionStrings { get; set; }
        public PaymentFeeEntity PaymentFee { get; set; }
        public int Take { get; set; }
    }
}
