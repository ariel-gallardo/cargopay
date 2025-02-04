using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class GuidEntity
    {
        private string _id = string.Empty;

        [Column("id")]
        [Key]
        public string Id { get => _id; 
            set {
                Guid temp;
                if (Guid.TryParse(value, out temp))
                    _id = value;
                else
                    _id = string.Empty;
            } 
        }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public override bool Equals(object obj)
        => obj != null && !string.IsNullOrEmpty(Id) && Id == ((StringEntity)obj).Id && obj is StringEntity;
    }
}
