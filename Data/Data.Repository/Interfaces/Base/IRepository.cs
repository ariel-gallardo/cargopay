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
        IQueryable<T> Where(Expression<Func<T, bool>> whereExpression);
        IQueryable<T> WhereSoftDeleted(Expression<Func<T, bool>> whereExpression);
        #endregion

        IUnitOfWork UnitOfWork { get; set; }
        public bool Exists(dynamic id);
        bool ExistsSoftDeleted(dynamic id);
        Task<bool> Restore(dynamic id);    
        void Detach(T entity);
    }
}
