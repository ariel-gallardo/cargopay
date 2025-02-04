using System.Text.RegularExpressions;
using Infraestructure;

namespace Application
{
    public class PaymentFeesServices : IPaymentFeesServices
    {
        private readonly AppSettings _appSettings;
        private double _currentUfe;
        private ParameterizedThreadStart _pThread;
        private Random _random;

        public PaymentFeesServices(AppSettings appSettings)
        {
            _appSettings = appSettings;
            _random = new Random();
            _pThread = new ParameterizedThreadStart(CheckCurrent);
            _pThread.Invoke(this);
        }

        public AppSettings.PaymentFeeEntity Config => _appSettings.PaymentFee;

        public async Task CheckNextValue() 
        {
            while (RunningThread)
            {
                var intPart = _random.Next(Config.MinValue, Config.MaxValue);
                var decPart = _random.Next(0, 99);
                _currentUfe = double.Parse($"{intPart},{decPart}");
                await Task.Delay(SleepTime);
            }
        }

        public int SleepTime
        {
            get
            {
                var cValue = Config.SleepTime.Trim().Replace(" ",string.Empty).ToLower();
                var number = Regex.Replace(cValue, @"[^0-9]", string.Empty);
                var type = Regex.Replace(cValue, @"[^a-zA-Z]", string.Empty);
                int timeParsed = int.Parse(number);
                switch (type)
                {
                    case "ms":
                        return timeParsed;
                        break;
                    case "s":
                        return timeParsed * 1000;
                        break;
                    case "m":
                        return timeParsed * 60000;
                        break;
                    case "h":
                        return timeParsed * 3600000;
                        break;
                    default:
                        throw new Exception("Check Sleep Time - Type");
                        break;
                }
            }
        }

        public double CurrentUFE => _currentUfe;

        private void CheckCurrent(object? data)
        {
            CheckNextValue();
        }

        public bool RunningThread { get; set; } = true;

        ~PaymentFeesServices()
        {
            RunningThread = false;
        }
    }
}
