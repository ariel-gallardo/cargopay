using System.Linq.Expressions;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
    }
}
