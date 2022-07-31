using FluentValidation;

namespace StrategyManager.Contracts.Strategies.Tickets.Validation
{
    /// <summary>
    /// Tickets request validator
    /// </summary>
    public class AddTicketRequestValidator : AbstractValidator<AddTicketRequest>
    {
        /// <summary>
        /// Validation rules
        /// </summary>
        public AddTicketRequestValidator()
        {
            RuleFor(x => x.Code).NotNull();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
