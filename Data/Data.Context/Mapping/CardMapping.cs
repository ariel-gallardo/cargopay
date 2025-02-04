using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    internal static class CardMapping
    {
        internal static void Map<T>(this ModelBuilder modelBuilder) where T : Card
        {
            modelBuilder.MapBaseString<T>();
            modelBuilder.Entity<T>().Property(x => x.Id).HasDefaultValueSql(@"RIGHT(REPLICATE('0', 15) + CAST(CAST(RAND(CHECKSUM(NEWID())) * 1000000000000000 AS BIGINT) AS VARCHAR(15)), 15)");
            modelBuilder.Entity<T>().ToTable("cards");
            modelBuilder.Entity<T>().Property(x => x.Balance).HasColumnName("balance").HasDefaultValue(0.00);
            modelBuilder.Entity<T>().Property(x => x.UserId).HasColumnName("user_id").IsRequired(true);
        }

        public static void MapRelationShips<T>(this ModelBuilder modelBuilder) where T : Card
        {
            
        }
    }
}
