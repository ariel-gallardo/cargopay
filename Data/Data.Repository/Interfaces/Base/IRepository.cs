using Domain;
using System.Linq.Expressions;

namespace Data
{
    public interface IRepository<T>
    {
        Task<int> Insert(T entity);
        Task<int> Insert(IList<T> entity);
        Task<int> Insert(IEnumerable<T> entity);
        Task<int> Update(T entity);
        Task<int> Update(IList<T> entity);
        Task<int> Update(IEnumerable<T> entity);
        Task<bool> Delete(dynamic id);
        Task<int> Delete(T entity);
        Task<int> Delete(IList<T> entity);
        Task<int> Delete(IEnumerable<T> entity);

        #region IQueryable
        IQueryable<T> Where(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false);
        IQueryable<T> WhereSoftDeleted(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false);
        IQueryable<T> WhereOrderByExpressions(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions);
        IQueryable<T> WhereOrderByExpressionsSoftDeleted(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions);
        (int, IQueryable<T>) WhereAsPaginateQuerie(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1);
        #endregion

        #region IQueryable Custom
        IQueryable<Output> Where<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false);
        IQueryable<Output> WhereSoftDeleted<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false);
        IQueryable<Output> WhereOrderByExpressions<Output>(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions);
        IQueryable<Output> WhereOrderByExpressionsSoftDeleted<Output>(Expression<Func<T, bool>> whereExpression, params (Expression<Func<T, object>>, bool)[] orderByExpressions);
        (int, IQueryable<Output>) WhereAsPaginateQuerie<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1);
        #endregion

        #region Paginate
        Task<Pagination<T>> WhereAsPaginateListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1);
        Task<Pagination<T>> WhereAsPaginateWithTakeAsListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1, int take = 10);
        Task<Pagination<T>> WhereAsPaginateWithTakeOrderByAsListAsync(Expression<Func<T, bool>> whereExpression, int page, int take, params (Expression<Func<T, object>> ordenarPor, bool)[] orderByExpressions);
        #endregion

        #region Paginate Custom
        Task<Pagination<Output>> WhereAsPaginateListAsync<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1);
        Task<Pagination<Output>> WhereAsPaginateWithTakeAsListAsync<Output>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1, int take = 10);
        Task<Pagination<Output>> WhereAsPaginateWithTakeOrderByAsListAsync<Output>(Expression<Func<T, bool>> whereExpression, int page, int take, params (Expression<Func<T, object>> ordenarPor, bool)[] orderByExpressions);
        #endregion

        IUnitOfWork UnitOfWork { get; set; }
        public bool Exists(dynamic id);
        bool ExistsSoftDeleted(dynamic id);
        Task<bool> Restore(dynamic id);    
        void Detach(T entity);
    }
}
