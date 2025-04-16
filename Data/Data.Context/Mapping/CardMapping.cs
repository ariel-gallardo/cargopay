using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    internal static class CardMapping
    {
        internal static void Map<T>(this ModelBuilder modelBuilder) where T : Card
        {
            modelBuilder.MapBaseString<T>();
            modelBuilder.Entity<T>().ToTable("cards");
            modelBuilder.Entity<T>().Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
            modelBuilder.Entity<T>().Property(x => x.Balance).HasColumnName("balance").HasDefaultValue(0.00);
            modelBuilder.Entity<T>().Property(x => x.UserId).HasColumnName("user_id").IsRequired(true);
        }

        public static void MapRelationShips<T>(this ModelBuilder modelBuilder) where T : Card
        {
            
        }
    }
}
