using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public partial class CargoPayContext : DbContext
    {
        public CargoPayContext(DbContextOptions<CargoPayContext> options) :
        base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder mB)
        {
            base.OnModelCreating(mB);
            MapEntities(ref mB);
            MapRelationShips(ref mB);
        }

        private void MapEntities(ref ModelBuilder mB)
        {
            mB.Map<User>();
        }

        private void MapRelationShips(ref ModelBuilder mB)
        {

        }

        public override int SaveChanges()
        {

            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
                if (entity.Entity is BigIntEntity || entity.Entity is StringEntity || entity.Entity is GuidEntity)
                    entity.Property("CreatedAt").CurrentValue = DateTime.UtcNow;

            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified && e.Property("DeletedAt").CurrentValue == e.Property("DeletedAt").OriginalValue))
                if ((entity.Entity is BigIntEntity || entity.Entity is StringEntity || entity.Entity is GuidEntity))
                    entity.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;

            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
                if (entity.Entity is BigIntEntity || entity.Entity is StringEntity || entity.Entity is GuidEntity)
                {
                    entity.Property("DeletedAt").CurrentValue = DateTime.UtcNow;
                    entity.State = EntityState.Modified;
                }

            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
                if (entity.Entity is BigIntEntity || entity.Entity is StringEntity || entity.Entity is GuidEntity)
                    entity.Property("CreatedAt").CurrentValue = DateTime.UtcNow;


            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified && e.Property("DeletedAt").CurrentValue == e.Property("DeletedAt").OriginalValue))
                if ((entity.Entity is BigIntEntity || entity.Entity is StringEntity || entity.Entity is GuidEntity))
                    entity.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;

            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
                if (entity.Entity is BigIntEntity || entity.Entity is StringEntity || entity.Entity is GuidEntity)
                {
                    entity.Property("DeletedAt").CurrentValue = DateTime.UtcNow;
                    entity.State = EntityState.Modified;
                }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
