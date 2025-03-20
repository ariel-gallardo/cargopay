using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    internal static class MapBaseExtensions
    {
        internal static ModelBuilder MapBaseString<T>(this ModelBuilder modelBuilder) where T : StringEntity
        {
            modelBuilder.Entity<T>().HasKey(x => x.Id);
            modelBuilder.Entity<T>().Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now() AT TIME ZONE 'UTC'");
            modelBuilder.Entity<T>().Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired(false);
            modelBuilder.Entity<T>().Property(x => x.DeletedAt).HasColumnName("deleted_at").IsRequired(false);
            modelBuilder.Entity<T>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<T>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<T>().HasIndex(x => x.DeletedAt);
            modelBuilder.Entity<T>().HasQueryFilter(x => x.DeletedAt == null);
            return modelBuilder;
        }
        internal static ModelBuilder MapBaseGuid<T>(this ModelBuilder modelBuilder) where T : GuidEntity 
        {
            modelBuilder.Entity<T>().HasKey(x => x.Id);
            modelBuilder.Entity<T>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<T>().Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now() AT TIME ZONE 'UTC'");
            modelBuilder.Entity<T>().Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired(false);
            modelBuilder.Entity<T>().Property(x => x.DeletedAt).HasColumnName("deleted_at").IsRequired(false);
            modelBuilder.Entity<T>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<T>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<T>().HasIndex(x => x.DeletedAt);
            modelBuilder.Entity<T>().HasQueryFilter(x => x.DeletedAt == null);
            return modelBuilder;
        }

        internal static ModelBuilder MapBaseBigInt<T>(this ModelBuilder modelBuilder) where T : BigIntEntity
        {
            modelBuilder.Entity<T>().HasKey(x => x.Id);
            modelBuilder.Entity<T>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<T>().Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now() AT TIME ZONE 'UTC'");
            modelBuilder.Entity<T>().Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired(false);
            modelBuilder.Entity<T>().Property(x => x.DeletedAt).HasColumnName("deleted_at").IsRequired(false);
            modelBuilder.Entity<T>().HasIndex(x => x.CreatedAt);
            modelBuilder.Entity<T>().HasIndex(x => x.UpdatedAt);
            modelBuilder.Entity<T>().HasIndex(x => x.DeletedAt);
            modelBuilder.Entity<T>().HasQueryFilter(x => x.DeletedAt == null);
            return modelBuilder;
        }
    }
}
