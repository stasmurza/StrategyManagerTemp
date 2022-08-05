using FluentValidation;
using MongoDB.Bson;

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
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Id)
                .Must(x => ObjectId.TryParse(x, out _))
                .When(x => !string.IsNullOrEmpty(x.Id))
                .WithMessage("Id must be a valid hexadecimal value");
        }
    }
}
