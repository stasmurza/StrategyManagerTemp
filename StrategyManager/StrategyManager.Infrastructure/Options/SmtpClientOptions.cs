namespace StrategyManager.Infrastructure.Options
{
    public class SmtpClientOptions
    {
        public string SmtpHost { get; set; } = String.Empty;
        public int SmtpPort { get; set; } = default;
        public string SmtpLogin { get; set; } = String.Empty;
        public string SmtpPassword { get; set; } = String.Empty;
        public string EmailFromAddress { get; set; } = String.Empty;
    }
}
