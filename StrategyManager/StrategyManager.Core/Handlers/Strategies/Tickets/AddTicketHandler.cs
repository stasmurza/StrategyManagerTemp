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
    public class AddTicketHandler : IRequestHandler<AddTicketInput, Unit> 
    {
        private readonly IRepository<Strategy> repository;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public AddTicketHandler(
            IRepository<Strategy> repository,
            IMapper mapper,
            ILogger<AddTicketHandler> logger)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(AddTicketInput input, CancellationToken cancellationToken)
        {
            var job = await repository.GetByIdAsync(input.JobId);
            if (job == null)
            {
                var message = $"Job with id {input.JobId} is not found";
                throw new NotFoundException(message)
                {
                    ErrorCode = ErrorCodes.NotFound
                };
            }
            
            if (job.Tickets.Any(i => i.Code == input.Code))
            {
                var message = $"Ticket with code {input.Code} has already been added";
                throw new ConflictException(message)
                {
                    ErrorCode = ErrorCodes.AlreadyExist
                };
            }

            var ticket = mapper.Map(input, new Ticket());
            job.Tickets.Add(ticket);
            
            await repository.UpdateAsync(job);

            return Unit.Value;
        }
    }
}
