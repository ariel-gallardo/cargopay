using AutoMapper;
using Domain;
using Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression)
        => _ctx.Set<T>().Where(whereExpression);

        public IQueryable<T> WhereSoftDeleted(Expression<Func<T, bool>> whereExpression)
        => Where(whereExpression).IgnoreQueryFilters();

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
            }
            catch (Exception e)
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
