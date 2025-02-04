using Domain;

namespace Application
{
    public interface IPasswordServices
    {
        string Encrypt(string password);
        public bool Ok(string password, string hashPassword);
        public (string, string) GenerateToken(User user);
    }
}
