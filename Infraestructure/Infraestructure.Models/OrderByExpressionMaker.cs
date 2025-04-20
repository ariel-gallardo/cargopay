using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Reflection;

namespace Infraestructure
{
    public static class CustomExpression
    {
        public static (Expression<Func<T, object>>, bool)[] OrderByExpressionMaker<T>(this string orderBy)
        {
            var result = new List<(Expression<Func<T, object>>, bool)>();
            if (!string.IsNullOrEmpty(orderBy))
            {
                var orders = orderBy.Split(',');
                foreach (var order in orders)
                {
                    var split = order.Split('=');
                    var exp = split[0]; exp = $"{exp[0].ToString().ToUpper()}{exp.Substring(1)}";
                    var asc = split[1].ToLowerInvariant() == "asc";
                    var param = Expression.Parameter(typeof(T), exp);
                    var prop = typeof(T).GetProperty(exp);
                    if (prop == null)
                        throw new ArgumentException($"La propiedad '{exp}' no existe en el tipo '{typeof(T).Name}'");
                    var propAccess = Expression.Property(param, prop);
                    Expression converted = Expression.Convert(propAccess, typeof(object));
                    result.Add((Expression.Lambda<Func<T, object>>(converted, param), asc));
                }
            }
            return result.ToArray();
        }
    }
}
