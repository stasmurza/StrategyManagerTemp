using FluentValidation;

namespace StrategyManager.Contracts.Strategies.Validation
{
    /// <summary>
    /// Start strategy request validator
    /// </summary>
    public class StartStrategyRequestRequestValidator : AbstractValidator<RunStrategyRequest>
    {
        /// <summary>
        /// Validation rules
        /// </summary>
        public StartStrategyRequestRequestValidator()
        {
        }
    }
}
