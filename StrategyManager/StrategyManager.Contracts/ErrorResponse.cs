namespace StrategyManager.Contracts
{
    /// <summary>
    /// Error response
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Code
        /// </summary>
        /// <example>400001</example>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
        /// <example>Conflict</example>
        public string Description { get; set; } = string.Empty;
    }
}
