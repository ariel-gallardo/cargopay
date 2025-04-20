using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.Context.Generators
{
    public class CardIdGenerator : ValueGenerator<string>
    {
        private static readonly Random _random = new();

        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
        {
            long numero = (long)(_random.NextDouble() * 1_000_000_000_000_000);
            return numero.ToString("D15"); // 15 dígitos con ceros a la izquierda
        }
    }
}
