namespace Infraestructure
{
    public partial class AppSettings
    {
        public class JwtEntity
        {
            public int HourExpirationTime { get; set; }
            public string SecretKey { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public bool ValidateIssuer { get; set; }
            public bool ValidateAudience { get; set; }
            public bool ValidateLifetime { get; set; }
        }
    }
}
