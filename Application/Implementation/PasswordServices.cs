using Domain;
using Infraestructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application
{
    public class PasswordServices : IPasswordServices
    {
        private readonly AppSettings _appSettings;

        public PasswordServices(AppSettings appSettings)
        {
            _appSettings = appSettings;    
        }
        public string Encrypt(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
        public (string, string) GenerateToken(User user)
        {
            var symmetricKey = Encoding.UTF8.GetBytes(_appSettings.JWT.SecretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var expirationTime = DateTime.UtcNow.AddHours(_appSettings.JWT.HourExpirationTime).ToString("dd-MM-yyyy HH:mm:ss");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Expiration, $"{expirationTime}"),
            }),
                Expires = DateTime.UtcNow.AddHours(_appSettings.JWT.HourExpirationTime),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(symmetricKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), expirationTime);
        }

        public bool Ok(string password, string hashPassword)
        => BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }
}
