using FluentValidation;

namespace StrategyManager.Contracts.Strategies.Tickets.Validation
{
    /// <summary>
    /// Tickets request validator
    /// </summary>
    public class RemoveTicketRequestValidator : AbstractValidator<AddTicketRequest>
    {
        /// <summary>
        /// Validation rules
        /// </summary>
        public RemoveTicketRequestValidator()
        {
            RuleFor(x => x.Code).NotNull();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
