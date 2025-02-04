using Domain;
using System.Linq.Expressions;

namespace Data
{
    public interface IRepository<T> where T : class
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
        public bool Exists(dynamic id);
        IQueryable<T> Where(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false);
        IQueryable<T> WhereSoftDeleted(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderByExpression = null, bool ascending = false);
        bool ExistsSoftDeleted(dynamic id);
        Task<bool> Restore(dynamic id);
        (int,IQueryable<T>) WhereAsPaginateQuerie(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1);
        Task<Pagination<T>> WhereAsPaginateListAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> ordenarPor = null, bool ascendente = true, int page = 1);
        IUnitOfWork UnitOfWork { get; set; }
    }
}
