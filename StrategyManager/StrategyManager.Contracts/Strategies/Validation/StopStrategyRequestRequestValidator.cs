using FluentValidation;

namespace StrategyManager.Contracts.Strategies.Validation
{
    /// <summary>
    /// Stop strategy request validator
    /// </summary>
    public class StopStrategyRequestRequestValidator : AbstractValidator<StopStrategyRequest>
    {
        /// <summary>
        /// Validation rules
        /// </summary>
        public StopStrategyRequestRequestValidator()
        {
        }
    }
}
