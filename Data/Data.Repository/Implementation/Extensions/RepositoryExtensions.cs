using System.Linq.Expressions;
using AutoMapper;
using Domain;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
namespace Data
{
    public static class RepositoryExtensions
    {
        public static IQueryable<T> MultipleIncludes<T>(this IQueryable<T> querie, params Expression<Func<T, object>>[] includes) where T : class
        {
            foreach (var include in includes)
                querie = EntityFrameworkQueryableExtensions.Include(querie,include);
            return querie;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> querie, params (Expression<Func<T, object>> ordenarPor, bool)[] orderByExpressions)
        {
            for (var i = 0; i < orderByExpressions.Length; i++)
            {
                var (exp,asc) = orderByExpressions[i];
                if (i == 0)
                    querie = asc ? Queryable.OrderBy(querie, exp) : Queryable.OrderByDescending(querie, exp);
                else
                    querie = asc ? Queryable.ThenBy((IOrderedQueryable<T>)querie, exp) : Queryable.ThenByDescending((IOrderedQueryable<T>)querie, exp);
            }
            return querie;
        }

        public static async Task<Pagination<T>> AsPaginate<T>(this IQueryable<T> querie, int page, int take)
        {
            var resultList = new List<T>();
            var count = await querie.CountAsync();
            if (page > 1)
                resultList.AddRange(await querie.Skip((page - 1) * take).Take(take).ToListAsync());
            else
                resultList.AddRange(await querie.Take(take).ToListAsync());
            return Pagination<T>.Crear(resultList, count, page);
        }

        public static async Task<Pagination<Output>> AsPaginate<T,Output>(this IQueryable<T> querie, IMapper mapper, int page, int take)
        {
            var resultList = new List<Output>();
            var count = await querie.CountAsync();
            if (page > 1)
                resultList.AddRange(await querie.Skip((page - 1) * take).Take(take).Select(x => mapper.Map<Output>(x)).ToListAsync());
            else
                resultList.AddRange(await querie.Take(take).Select(x => mapper.Map<Output>(x)).ToListAsync());
            return Pagination<T>.Crear(resultList, count, page);
        }

        public static IQueryable<T> AddSearchByFilters<T>(this IQueryable<T> querie, string searchByFilters)
        {
            var exps = searchByFilters.SearchByExpressionMaker<T>();

            if (exps == null || exps.Length == 0)
                return querie;

            var combined = exps[0];
            for (int i = 1; i < exps.Length; i++)
            {
                combined = CombineWithAnd(combined, exps[i]);
            }

            return querie.Where(combined);
        }

        private static Expression<Func<T, bool>> CombineWithAnd<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var leftVisitor = new ReplaceParameterVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            var body = Expression.AndAlso(left!, right!);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly ParameterExpression _newParam;

            public ReplaceParameterVisitor(ParameterExpression oldParam, ParameterExpression newParam)
            {
                _oldParam = oldParam;
                _newParam = newParam;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParam ? _newParam : base.VisitParameter(node);
            }
        }
    }
}
