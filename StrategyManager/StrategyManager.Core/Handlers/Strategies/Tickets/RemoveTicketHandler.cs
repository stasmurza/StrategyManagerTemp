using AutoMapper;
using StrategyManager.Core.Exceptions;
using StrategyManager.Core.Models.Handlers.Strategies;
using StrategyManager.Core.Models.Handlers.Strategies.Tickets;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace StrategyManager.Core.Handlers.Strategies.Tickets
{
    public class RemoveTicketHandler : IRequestHandler<RemoveTicketInput, Unit> 
    {
        private readonly IRepository<Strategy> repository;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public RemoveTicketHandler(
            IRepository<Strategy> repository,
            IMapper mapper,
            ILogger<AddTicketHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(RemoveTicketInput input, CancellationToken cancellationToken)
        {
            var strategy = await repository.GetByIdAsync(input.StrategyId);
            if (strategy == null)
            {
                var message = $"Strategy with id {input.StrategyId} is not found";
                throw new NotFoundException(message) { ErrorCode = ErrorCodes.NotFound };
            }

            var ticket = strategy.Tickets.FirstOrDefault(i => i.Code == input.Code);
            if (ticket is null)
            {
                var message = $"Ticket with code {input.Code} was not found";
                throw new NotFoundException(message)
                {
                    ErrorCode = ErrorCodes.NotFound
                };
            }


            strategy.Tickets.Remove(ticket);
            await repository.UpdateAsync(strategy);

            return Unit.Value;
        }
    }
}
