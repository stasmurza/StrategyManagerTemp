namespace StrategyManager.Contracts
{
    /// <summary>
    /// Error status codes
    /// </summary>
    public class ErrorStatusCodes
    {
        /// <summary>
        /// Bad request error code
        /// </summary>
        public const int BadRequest = 400000;

        /// <summary>
        /// Unathorized error code
        /// </summary>
        public const int Unathorized = 401000;

        /// <summary>
        /// Server internal error
        /// </summary>
        public const int ServerInternalError = 500000;
    }
}
