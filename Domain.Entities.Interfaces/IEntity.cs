using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public interface IEntity<TId>
    {
        TId Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}


