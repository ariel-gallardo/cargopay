using Data;

namespace Data
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        ICardRepository Card { get; }
        void BeginTransaction();
        Task BeginTransactionAsync();
        void CommitTransaction();
        Task CommitTransactionAsync();
        void RollbackTransaction();
        Task RollbackTransactionAsync();
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
