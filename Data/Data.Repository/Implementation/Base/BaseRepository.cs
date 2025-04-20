using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public IQueryable<T> Where(Expression<Func<T, bool>> whereExpression)
        => _repository.Where(whereExpression);

        public IQueryable<T> WhereSoftDeleted(Expression<Func<T, bool>> whereExpression)
        => _repository.WhereSoftDeleted(whereExpression);
    }
}
