namespace Domain
{
    public class CardInfoDTO
    {
        public string CardId { get; set; }
        public long UserId { get; set; }
        public double Balance { get; set; }
        public string Client { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
