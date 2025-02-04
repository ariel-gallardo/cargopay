namespace Domain
{
    public class User : BigIntEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual List<Card> Cards { get; set; }
    }
}
