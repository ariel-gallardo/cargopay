using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Infraestructure
{
    public class CustomEmailAddressAttribute : ValidationAttribute
    {
        // Propiedades
        public string? Email { get; set; }
        public string?[] Emails { get; set; }

        // Constructor para aceptar Email y Emails opcionalmente
        public CustomEmailAddressAttribute(string? email = null, string?[] emails = null)
            : base()
        {
            Email = email;
            Emails = emails;
        }

        public override bool IsValid(object value)
        {
            var invEmails = new List<string>();

            if (value == null)
                invEmails.Add("ANY");
            else if (value is string email)
            {
                Email = email;
                var regex = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w{2,}$");
                if (!regex.IsMatch(Email)) invEmails.Add(Email);
            }
            else if (value is string[] emails)
            {
                Emails = emails;
                foreach (var e in Emails)
                {
                    var regex = new Regex(@"^[\w\.-]+@[\w\.-]+\.\w{2,}$");
                    if (!regex.IsMatch(e))
                        invEmails.Add(e);
                }
            }

            if(invEmails.Count > 0) ErrorMessage = Messages.InvalidEmail(
                Emails?.Length > 0 ? Emails : 
                new string[]{!string.IsNullOrWhiteSpace(Email)
                ? Email : "ANY"}
            );

            return invEmails.Count == 0;
        }
    }
}
