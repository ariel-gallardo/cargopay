using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    internal static class UserMapping
    {
        internal static void Map<T>(this ModelBuilder modelBuilder) where T : User
        {
            modelBuilder.MapBaseBigInt<T>();
            modelBuilder.Entity<T>().ToTable("users");
            modelBuilder.Entity<T>().Property(x => x.Name).HasColumnName("name").IsRequired(false);
            modelBuilder.Entity<T>().Property(x => x.Email).HasColumnName("email").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<T>().Property(x => x.Password).HasColumnName("password").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<T>().HasIndex(x => x.Email).IsUnique(true);
        }

        public static void MapRelationShips<T>(this ModelBuilder modelBuilder) where T : User
        {
            modelBuilder.Entity<T>().HasMany(x => x.Cards).WithOne().HasForeignKey(x => x.UserId);
        }
    }
}
