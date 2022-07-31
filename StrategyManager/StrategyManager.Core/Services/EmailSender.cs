using StrategyManager.Core.Models.Options;
using StrategyManager.Core.Proxies.Clients;
using StrategyManager.Core.Services.Abstractions;
using Microsoft.Extensions.Options;

namespace StrategyManager.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ISmtpClient smtpClient;
        private readonly EmailOptions options;

        public EmailSender(ISmtpClient smtpClient, IOptions<EmailOptions> options)
        {
            this.smtpClient = smtpClient;
            this.options = options.Value;
        }

        public async Task SendAsync(string recipient, string subject, string body)
        {
            if (string.IsNullOrEmpty(options.EmailFromAddress)) throw new ArgumentNullException(nameof(options.EmailFromAddress));
            if (string.IsNullOrEmpty(recipient)) throw new ArgumentNullException(nameof(recipient));

            await smtpClient.SendMailAsync(options.EmailFromAddress, recipient, subject, body);
        }
    }
}
