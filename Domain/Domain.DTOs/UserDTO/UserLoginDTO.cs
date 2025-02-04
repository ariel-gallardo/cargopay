using System.ComponentModel.DataAnnotations;
using Infraestructure;
namespace Domain
{
    public class UserLoginDTO
    {
        [CustomEmailAddress]
        public string UserEmail { get; set; }
        [CustomRequired(ErrorMessageResourceType = typeof(string))]
        public string UserPassword { get; set; }
    }
}
