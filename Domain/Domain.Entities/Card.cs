using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Card : StringEntity
    {
        public long UserId;
        public double Balance { get; set; }
        public virtual User User { get; set; }
    }
}
