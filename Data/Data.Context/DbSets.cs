using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public partial class CargoPayContext
    {
        public virtual DbSet<User> User { get; set; }
    }
}
