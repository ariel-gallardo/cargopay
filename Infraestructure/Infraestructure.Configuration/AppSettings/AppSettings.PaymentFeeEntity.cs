namespace Infraestructure
{
    public partial class AppSettings
    {
        public class PaymentFeeEntity
        {
            public int MinValue { get; set; }
            public int MaxValue { get; set; }
            public string SleepTime { get; set; }
        }
    }
}
