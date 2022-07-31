namespace StrategyManager.Core.Services.Abstractions
{
    public interface IEmailSender
    {
        public Task SendAsync(string email, string subject, string htmlMessage);
    }
}
