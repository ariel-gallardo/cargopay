using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Private
        private readonly CargoPayContext _ctx;
        private readonly IUserRepository _userRepository;
        #endregion

        #region Public
        public IUserRepository User { get => _userRepository; }
        public CargoPayContext Context { get => _ctx; }
        #endregion

        #region Constructor
        public UnitOfWork(
            CargoPayContext ctx,
            IUserRepository userRepository
            )
        {
            _userRepository = userRepository;
            _ctx = ctx;
            AssignUnitOfWork();
        }
        #endregion
        public void Dispose()
        {
            _transaction?.Dispose();
            _ctx.Dispose();
        }

        private void AssignUnitOfWork()
        {
            _userRepository.UnitOfWork = this;
        }

        private IDbContextTransaction _transaction;

        public IDbContextTransaction Transaction { get => _transaction; }


        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _ctx.Database.BeginTransaction();
            }
        }
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _ctx.Database.BeginTransactionAsync();
            }
        }
        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Commit();
                }
                catch (Exception ex) 
                {
                    RollbackTransaction();
                    throw ex;
                }
                finally
                {
                    
                }
            }
        }
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                try
                {
                    await _transaction.CommitAsync();
                }
                catch(Exception ex)
                {
                    await RollbackTransactionAsync();
                    throw ex;
                }
                finally
                {
                    
                }
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void SaveChanges() => _ctx.SaveChanges();

        public async Task SaveChangesAsync() => await _ctx.SaveChangesAsync();
        
    }
}
