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
            var job = await repository.GetByIdAsync(input.JobId);
            if (job == null)
            {
                var message = $"Job with id {input.JobId} is not found";
                throw new NotFoundException(message) { ErrorCode = ErrorCodes.NotFound };
            }

            var ticket = job.Tickets.FirstOrDefault(i => i.Code == input.Code);
            if (ticket is null)
            {
                var message = $"Ticket with code {input.Code} was not found";
                throw new NotFoundException(message)
                {
                    ErrorCode = ErrorCodes.NotFound
                };
            }

            
            job.Tickets.Remove(ticket);
            await repository.UpdateAsync(job);

            return Unit.Value;
        }
    }
}
