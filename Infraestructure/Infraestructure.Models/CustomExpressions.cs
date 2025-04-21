using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Reflection;
using System.Data.Common;
using System.Text;

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

        private static string TranslateExp(string exp)
        {
            switch (exp)
            {
                case "client":
                    return "User.Name";
                case "createdAt":
                    return "CreatedAt";
                case "cardId":
                    return "Id";
                case "balance":
                    return "Balance";
                case "userId":
                    return "User.Id";
                default:
                    return exp;
            }
        }

        private static PropertyInfo GetNestedProperty(Type type, string propertyPath)
        {
            PropertyInfo prop = null;
            foreach (var part in propertyPath.Split('.'))
            {
                prop = type.GetProperty(part);
                if (prop == null) return null;
                type = prop.PropertyType;
            }
            return prop;
        }

        public static Expression<Func<T, bool>>[] SearchByExpressionMaker<T>(this string searchBy)
        {
            var result = new List<Expression<Func<T, bool>>>();
            if (!string.IsNullOrEmpty(searchBy))
            {
                var orders = searchBy.Split(',');
                var searchByData = new List<(string, string, string)>();

                foreach (var order in orders)
                {
                    var split = order.Split('=');
                    var exp = split[0];
                    var subSplit = split[1].Split('|');
                    var val = subSplit[0];
                    var action = subSplit[1];

                    searchByData.Add((TranslateExp(exp), val, action));
                }

                foreach (var (exp, val, action) in searchByData)
                {
                    var param = Expression.Parameter(typeof(T), "x");
                    Expression left = param;

                    // Manejo de propiedades anidadas
                    if (exp.Contains("."))
                    {
                        foreach (var property in exp.Split('.'))
                        {
                            left = Expression.Property(left, property);
                        }
                    }
                    else
                    {
                        var prop = typeof(T).GetProperty(exp);
                        if (prop == null)
                            throw new ArgumentException($"La propiedad '{exp}' no existe en el tipo '{typeof(T).Name}'");
                        left = Expression.Property(param, prop);
                    }

                    DateTime? r = null;
                    if (left.Type == typeof(DateTime))
                    {
                        r = DateTime.Parse(val, null, System.Globalization.DateTimeStyles.RoundtripKind).ToUniversalTime();
                    }
                    
                    object value = Convert.ChangeType(r != null ? r.Value : val, left.Type);

                    var right = Expression.Constant(value, left.Type);

                    Expression op = null;

                    switch (action)
                    {
                        // Strings
                        case "startsWith":
                            op = Expression.Call(left, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), right);
                            break;
                        case "contains":
                            op = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right);
                            break;
                        case "notContains":
                            op = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }), right));
                            break;
                        case "endsWith":
                            op = Expression.Call(left, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), right);
                            break;

                        // Comunes
                        case "equals":
                        case "dateIs":
                            op = Expression.Equal(left, right);
                            break;
                        case "notEquals":
                            op = Expression.NotEqual(left, right);
                            break;

                        // Números
                        case "lt":
                            op = Expression.LessThan(left, right);
                            break;
                        case "lte":
                            op = Expression.LessThanOrEqual(left, right);
                            break;
                        case "gt":
                            op = Expression.GreaterThan(left, right);
                            break;
                        case "gte":
                            op = Expression.GreaterThanOrEqual(left, right);
                            break;

                        // Fechas
                        case "before":
                        case "dateBefore":
                            op = Expression.LessThan(left, right);
                            break;
                        case "after":
                        case "dateAfter":
                            op = Expression.GreaterThan(left, right);
                            break;
                    }

                    result.Add(Expression.Lambda<Func<T, bool>>(op, param));
                }
            }
            return result.ToArray();
        }

    }
}
