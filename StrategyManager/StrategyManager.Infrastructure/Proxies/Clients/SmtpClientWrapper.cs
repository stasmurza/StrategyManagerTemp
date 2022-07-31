using StrategyManager.Core.Proxies.Clients;
using StrategyManager.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace StrategyManager.Infrastructure.Proxies.Clients
{
    public class SmtpClientWrapper : ISmtpClient
    {
        private readonly SmtpClientOptions options;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = default;
        public string SmtpLogin { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;

        public SmtpClientWrapper(IOptions<SmtpClientOptions> options)
        {
            this.options = options.Value;
        }

        public async Task SendMailAsync(string from, string recipients, string subject, string body)
        {
            if (string.IsNullOrEmpty(options.SmtpHost)) throw new ArgumentNullException(nameof(options.SmtpHost));
            if (options.SmtpPort == default) throw new ArgumentNullException(nameof(options.SmtpPort));
            if (string.IsNullOrEmpty(options.SmtpLogin)) throw new ArgumentNullException(nameof(options.SmtpLogin));
            if (string.IsNullOrEmpty(options.SmtpPassword)) throw new ArgumentNullException(nameof(options.SmtpPassword));

            var message = new MailMessage(from, recipients, subject, body);
            message.IsBodyHtml = true;

            using var smtpClient = new SmtpClient(SmtpHost)
            {
                Port = options.SmtpPort,
                Credentials = new NetworkCredential(options.SmtpLogin, options.SmtpPassword),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
