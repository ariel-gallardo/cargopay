using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Reflection;


namespace Infraestructure
{

    public class CustomRequiredAttribute : ValidationAttribute
    {
        public CustomRequiredAttribute()
        {
            ErrorMessage = "{0} IS_REQUIRED";
        }


        public override bool IsValid(object value)
        {
            var propertyType = ErrorMessageResourceType;

            if (value == null && IsNullableType(propertyType))
            {
                return true;
            }

            if (value is string strValue && string.IsNullOrWhiteSpace(strValue))
            {
                return false;
            }

            return value != null; 
        }
        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessage, name);
        }

        private bool IsNullableType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (type == typeof(string))
            {
                return true;
            }

            return Nullable.GetUnderlyingType(type) != null;
        }
    }

}
