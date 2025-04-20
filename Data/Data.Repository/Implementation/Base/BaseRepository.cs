using System.Linq.Expressions;
using Domain;

namespace Data
{
    public abstract class BaseRepository<T> : IRepository<T>
    {
        private IRepository<T> _repository;

        public BaseRepository(IRepository<T> repository)
        {
            _repository = repository;
        }

        public IUnitOfWork UnitOfWork
        {
            get => _repository.UnitOfWork;
            set => _repository.UnitOfWork = value;
        }

        public async Task<bool> Delete(dynamic id)
            => await _repository.Delete(id);

        public async Task<int> Delete(T entity)
            => await _repository.Delete(entity);

        public async Task<int> Delete(IList<T> entity)
            => await _repository.Delete(entity);

        public async Task<int> Delete(IEnumerable<T> entity)
            => await _repository.Delete(entity);

        public void Detach(T entity)
            => _repository.Detach(entity);

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

        public async Task<bool> Restore(dynamic id)
            => await _repository.Restore(id);

        public async Task<int> Update(T entity)
            => await _repository.Update(entity);

        public async Task<int> Update(IList<T> entity)
            => await _repository.Update(entity);

        public async Task<int> Update(IEnumerable<T> entity)
        => await _repository.Update(entity);

        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
            => _repository.Where(whereExpression, orderByExpression, ascending);

        public IQueryable<Output> Where<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
            => _repository.Where<Output>(whereExpression, orderByExpression, ascending);

        public async Task<Pagination<T>> WhereAsPaginateListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
            => await _repository.WhereAsPaginateListAsync(whereExpression, ordenarPor, ascendente, page);

        public async Task<Pagination<Output>> WhereAsPaginateListAsync<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
            => await _repository.WhereAsPaginateListAsync<Output>(whereExpression, ordenarPor, ascendente, page);

        public (int, IQueryable<T>) WhereAsPaginateQuerie(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
            => _repository.WhereAsPaginateQuerie(whereExpression, ordenarPor, ascendente, page);

        public (int, IQueryable<Output>) WhereAsPaginateQuerie<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1)
            => _repository.WhereAsPaginateQuerie<Output>(whereExpression, ordenarPor, ascendente, page);

        public async Task<Pagination<T>> WhereAsPaginateWithTakeAsListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1, int take = 10)
            => await _repository.WhereAsPaginateWithTakeAsListAsync(whereExpression, ordenarPor, ascendente, page, take);

        public async Task<Pagination<Output>> WhereAsPaginateWithTakeAsListAsync<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1, int take = 10)
            => await _repository.WhereAsPaginateWithTakeAsListAsync<Output>(whereExpression, ordenarPor, ascendente, page, take);

        public async Task<Pagination<T>> WhereAsPaginateWithTakeOrderByAsListAsync(Expression<Func<T, bool>> whereExpression, int page, int take, params (Expression<Func<T, object>> ordenarPor, bool)[] orderByExpressions)
            => await _repository.WhereAsPaginateWithTakeOrderByAsListAsync(whereExpression, page, take, orderByExpressions);

        public async Task<Pagination<Output>> WhereAsPaginateWithTakeOrderByAsListAsync<Output>(Expression<Func<T, bool>> whereExpression, int page, int take, params (Expression<Func<T, object>> ordenarPor, bool)[] orderByExpressions)
            => await _repository.WhereAsPaginateWithTakeOrderByAsListAsync<Output>(whereExpression, page, take, orderByExpressions);

        public IQueryable<T> WhereOrderByExpressions(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions)
            => _repository.WhereOrderByExpressions(whereExpression, orderByExpressions);

        public IQueryable<Output> WhereOrderByExpressions<Output>(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions)
            => _repository.WhereOrderByExpressions<Output>(whereExpression, orderByExpressions);

        public IQueryable<T> WhereOrderByExpressionsSoftDeleted(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions)
            => _repository.WhereOrderByExpressionsSoftDeleted(whereExpression, orderByExpressions);

        public IQueryable<Output> WhereOrderByExpressionsSoftDeleted<Output>(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions)
            => _repository.WhereOrderByExpressionsSoftDeleted<Output>(whereExpression, orderByExpressions);

        public IQueryable<T> WhereSoftDeleted(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
            => _repository.WhereSoftDeleted(whereExpression, orderByExpression, ascending);

        public IQueryable<Output> WhereSoftDeleted<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false)
            => _repository.WhereSoftDeleted<Output>(whereExpression, orderByExpression, ascending);
    }
}
