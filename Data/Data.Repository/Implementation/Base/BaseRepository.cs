using System.Linq.Expressions;
using Domain;

namespace Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly IRepository<T> _repository;
        public IUnitOfWork UnitOfWork { get; set; }
        public BaseRepository(IRepository<T> repository)
        {
            _repository = repository;
        }
        public async Task<int> Delete(T entity)
        => await _repository.Delete(entity);
        

        public async Task<int> Delete(IList<T> entity)
        => await _repository.Delete(entity);
        

        public async Task<int> Delete(IEnumerable<T> entity)
        => await _repository.Delete(entity);

        public bool Exists(dynamic id)
        => _repository.Exists(id);
        public bool ExistsSoftDeleted(dynamic id)
        => _repository.ExistsSoftDeleted(id);

        public async Task<int> Insert(T entity)
        => await _repository.Insert(entity);
        

        public async Task<int> Insert(IList<T> entity)
        => await _repository.Insert(entity);
        

        public async Task<int> Insert(IEnumerable<T> entity)
        => await _repository.Insert(entity);
        

        public async Task<int> Update(T entity)
        => await _repository.Update(entity);
        

        public async Task<int> Update(IList<T> entity)
        => await _repository.Update(entity);
        

        public async Task<int> Update(IEnumerable<T> entity)
        => await _repository.Update(entity);
        

        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
        => _repository.Where(whereExpression, orderByExpression, ascending);

        public IQueryable<T> WhereSoftDeleted(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
        => _repository.WhereSoftDeleted(whereExpression, orderByExpression, ascending);

        public async Task<bool> Restore(dynamic id)
        => await _repository.Restore(id);

        public async Task<Pagination<T>> WhereAsPaginateListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
        => await _repository.WhereAsPaginateListAsync(whereExpression,ordenarPor, ascendente, page);

        public (int,IQueryable<T>) WhereAsPaginateQuerie(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
        => _repository.WhereAsPaginateQuerie(whereExpression, ordenarPor, ascendente, page);

        public async Task<bool> Delete(dynamic id)
        => await _repository.Delete(id);
    }
}
