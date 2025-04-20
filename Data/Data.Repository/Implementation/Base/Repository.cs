using AutoMapper;
using Domain;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Data
{
    public class Repository<T> : IRepository<T>
    where T : class
    {
        private readonly CargoPayContext _ctx;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
  
        public Repository(CargoPayContext ctx, AppSettings appSettings, IMapper mapper)
        {
            _ctx = ctx;
            _appSettings = appSettings;
            _mapper = mapper;
        }
        public async Task<int> Insert(T entity)
        {
            await _ctx.AddAsync(entity);
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> Insert(IList<T> entity)
        {
            if (entity?.Count > 0)
            {
                await _ctx.AddRangeAsync(entity);
                return await _ctx.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> Insert(IEnumerable<T> entity)
        {
            if (entity?.Count() > 0)
            {
                await _ctx.AddRangeAsync(entity);

            }
            return 0;
        }
        public async Task<int> Update(T entity)
        {
            _ctx.Update(entity);
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> Update(IList<T> entity)
        {
            if (entity?.Count > 0)
            {
                _ctx.UpdateRange(entity);
                return await _ctx.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> Update(IEnumerable<T> entity)
        {
            if (entity?.Count() > 0)
            {
                _ctx.UpdateRange(entity);
                return await _ctx.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<int> Delete(T entity)
        {
            _ctx.Remove(entity);
            return await _ctx.SaveChangesAsync();
        }

        public async Task<int> Delete(IList<T> entity)
        {
            if (entity?.Count > 0)
            {
                _ctx.RemoveRange(entity);
                return await _ctx.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> Delete(IEnumerable<T> entity)
        {
            if (entity?.Count() > 0)
            {
                _ctx.RemoveRange(entity);
                return await _ctx.SaveChangesAsync();
            }
            return 0;
        }

        #region IQueryable
        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
        {
            var expression = _ctx.Set<T>().Where(whereExpression);

            if(orderByExpression != null)
            expression = ascending ? expression.OrderBy(orderByExpression) : expression.OrderByDescending(orderByExpression);

            return expression;
        }
        public IQueryable<T> WhereSoftDeleted(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
        {
            IEntity<T> nullEntity;

            var nonActiveExpression = Expression.Lambda<Func<T, bool>>(
                Expression.NotEqual(
                    Expression.Property(whereExpression.Parameters[0], nameof(nullEntity.DeletedAt)),
                    Expression.Constant(null, typeof(DateTime?))
                ),
                whereExpression.Parameters
            );

            var whereBody = whereExpression.Body;
            var activeBody = nonActiveExpression.Body;

            var combinedBody = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(whereBody, activeBody),
                whereExpression.Parameters
            );

            return Where(combinedBody, orderByExpression, ascending).IgnoreQueryFilters();
        }

        public IQueryable<T> WhereOrderByExpressions(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions)
        {
            var expression = _ctx.Set<T>().Where(whereExpression);

            for (var i = 0; i < orderByExpressions.Length; i++)
            {
                var (exp, asc) = orderByExpressions[i];
                if (i == 0)
                    expression = asc ? expression.OrderBy(exp) : expression.OrderByDescending(exp);
                else
                    expression = asc ? (expression as IOrderedQueryable<T>).ThenBy(exp) : (expression as IOrderedQueryable<T>).ThenByDescending(exp);
            }
            return expression;
        }

        public IQueryable<T> WhereOrderByExpressionsSoftDeleted(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions)
        => WhereOrderByExpressions(whereExpression, orderByExpressions).IgnoreQueryFilters();

        public (int, IQueryable<T>) WhereAsPaginateQuerie(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
        {
            var resultList = new List<T>();
            var querie = Where(whereExpression, ordenarPor, ascendente);
            var count = querie.Count();

            if (page > 1)
            {
                querie = querie.Skip((page - 1) * _appSettings.Take).Take(_appSettings.Take);
            }
            else
                querie = querie.Take(_appSettings.Take);
            return (count, querie);
        }
        #endregion

        #region IQueryable Custom
        public IQueryable<Output> Where<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
        => Where(whereExpression, orderByExpression, ascending).Select(x => _mapper.Map<Output>(x));

        public IQueryable<Output> WhereSoftDeleted<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
        => WhereSoftDeleted(whereExpression, orderByExpression, ascending).Select(x => _mapper.Map<Output>(x));

        public IQueryable<Output> WhereOrderByExpressions<Output>(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions)
        => WhereOrderByExpressions(whereExpression, orderByExpressions).Select(x => _mapper.Map<Output>(x));

        public IQueryable<Output> WhereOrderByExpressionsSoftDeleted<Output>(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions)
        => WhereOrderByExpressionsSoftDeleted(whereExpression, orderByExpressions).Select(x => _mapper.Map<Output>(x));
        public (int, IQueryable<Output>) WhereAsPaginateQuerie<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
        {
            var resultList = new List<T>();
            var querie = Where<Output>(whereExpression, ordenarPor, ascendente);
            var count = querie.Count();

            if (page > 1)
            {
                querie = querie.Skip((page - 1) * _appSettings.Take).Take(_appSettings.Take);
            }
            else
                querie = querie.Take(_appSettings.Take);
            return (count, querie);
        }
        #endregion

        #region Paginate
        public async Task<Pagination<T>> WhereAsPaginateListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
        {
            var resultList = new List<T>();
            var querie = Where(whereExpression, ordenarPor, ascendente);
            var count = await querie.CountAsync();
            if (page > 1)
                resultList.AddRange(await querie.Skip((page - 1) * _appSettings.Take).Take(_appSettings.Take).ToListAsync());
            else
                resultList.AddRange(await querie.Take(_appSettings.Take).ToListAsync());
            return Pagination<T>.Crear(resultList, count,page);
        }
        public async Task<Pagination<T>> WhereAsPaginateWithTakeAsListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1, int take = 10)
        {
            var resultList = new List<T>();
            var querie = Where(whereExpression, ordenarPor, ascendente);
            var count = await querie.CountAsync();
            if (page > 1)
                resultList.AddRange(await querie.Skip((page - 1) * take).Take(take).ToListAsync());
            else
                resultList.AddRange(await querie.Take(take).ToListAsync());
            return Pagination<T>.Crear(resultList, count, page);
        }
        public async Task<Pagination<T>> WhereAsPaginateWithTakeOrderByAsListAsync(Expression<Func<T, bool>> whereExpression, int page, int take, params (Expression<Func<T, object>> ordenarPor, bool)[] orderByExpressions)
        {
            var resultList = new List<T>();
            var querie = WhereOrderByExpressions(whereExpression, orderByExpressions);
            var count = await querie.CountAsync();
            if (page > 1)
                resultList.AddRange(await querie.Skip((page - 1) * take).Take(take).ToListAsync());
            else
                resultList.AddRange(await querie.Take(take).ToListAsync());
            return Pagination<T>.Crear(resultList, count, page);
        }
        #endregion

        #region Paginate Custom
        public async Task<Pagination<Output>> WhereAsPaginateListAsync<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
        {
            var resultList = new List<Output>();
            var querie = Where<Output>(whereExpression, ordenarPor, ascendente);
            var count = await querie.CountAsync();
            if (page > 1)
                resultList.AddRange(await querie.Skip((page - 1) * _appSettings.Take).Take(_appSettings.Take).ToListAsync());
            else
                resultList.AddRange(await querie.Take(_appSettings.Take).ToListAsync());
            return Pagination<T>.Crear(resultList, count, page);
        }
        public async Task<Pagination<Output>> WhereAsPaginateWithTakeAsListAsync<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1, int take = 10)
        {
            var resultList = new List<Output>();
            var querie = Where<Output>(whereExpression, ordenarPor, ascendente);
            var count = await querie.CountAsync();
            if (page > 1)
                resultList.AddRange(await querie.Skip((page - 1) * take).Take(take).ToListAsync());
            else
                resultList.AddRange(await querie.Take(take).ToListAsync());
            return Pagination<T>.Crear(resultList, count, page);
        }
        public async Task<Pagination<Output>> WhereAsPaginateWithTakeOrderByAsListAsync<Output>(Expression<Func<T, bool>> whereExpression, int page, int take, params (Expression<Func<T, object>> ordenarPor, bool)[] orderByExpressions)
        {
            var resultList = new List<Output>();
            var querie = WhereOrderByExpressions<Output>(whereExpression, orderByExpressions);
            var count = await querie.CountAsync();
            if (page > 1)
                resultList.AddRange(await querie.Skip((page - 1) * take).Take(take).ToListAsync());
            else
                resultList.AddRange(await querie.Take(take).ToListAsync());
            return Pagination<T>.Crear(resultList, count, page);
        }
        #endregion

        public async Task<bool> Delete(object id)
        {
            try
            {
                var entity = await Where(x => (x as IEntity<T>).Id == id).FirstOrDefaultAsync();
                if (entity != null)
                {
                    _ctx.Remove(entity);
                    return true;
                }
                return false;
            }catch(Exception e)
            {
                throw new NotImplementedException("INVALID_TYPEOF_ENTITY_DELETE");
            }
        }
        public IUnitOfWork UnitOfWork { get; set; }
        public bool Exists(object id)
        => Where((x) => (x as IEntity<T>).Id == id).Take(1).Count() == 1;
        public bool ExistsSoftDeleted(object id)
        => WhereSoftDeleted(x => (x as IEntity<T>).Id == id).Take(1).Count() == 1;
        public async Task<bool> Restore(object id)
        {
            var entity = await WhereSoftDeleted(x => (x as IEntity<T>).Id == id).FirstOrDefaultAsync();
            if (entity == null) return false;
            (entity as IEntity<T>).DeletedAt = null;
            await Update(entity);
            return true;
        }
        public void Detach(T entity)
        {
            _ctx.Entry(entity).State = EntityState.Detached;
        }
    }
}
