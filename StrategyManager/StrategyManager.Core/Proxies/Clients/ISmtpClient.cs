namespace StrategyManager.Core.Proxies.Clients
{
    public interface ISmtpClient
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpLogin { get; set; }
        public string SmtpPassword { get; set; }

        public Task SendMailAsync(string from, string recipients, string subject, string body);
    }
}
