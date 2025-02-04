using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class BigIntEntity
    {
        [Column("id")]
        [Key]
        public long Id { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public override bool Equals(object obj)
        => obj != null && GetHashCode() == obj.GetHashCode() && obj is BigIntEntity;

        public override int GetHashCode()
        => Id.GetHashCode() + GetType().GetHashCode();
    }
}
