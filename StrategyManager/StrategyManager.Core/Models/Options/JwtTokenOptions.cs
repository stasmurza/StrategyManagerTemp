namespace StrategyManager.Core.Models.Options
{
    public class JwtTokenOptions
    {
        public string Secret { get; set; } = String.Empty;
        public int ExpirationTimeMinutes { get; set; }
    }
}
