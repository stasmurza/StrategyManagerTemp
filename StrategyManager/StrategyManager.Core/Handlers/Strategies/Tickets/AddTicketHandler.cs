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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public AddTicketHandler(
            IRepository<Strategy> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AddTicketHandler> logger)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(AddTicketInput input, CancellationToken cancellationToken)
        {
            var include = new Expression<Func<Strategy, object>>[]
            {
                strategy => strategy.Tickets,
            };

            var strategy = await repository.GetByIdAsync(input.StrategyId, );
            if (strategy == null)
            {
                var message = $"Strategy with id {input.StrategyId} is not found";
                throw new NotFoundException(message)
                {
                    ErrorCode = ErrorCodes.NotFound
                };
            }
            
            if (strategy.Tickets.Any(i => i.Code == input.Code))
            {
                var message = $"Ticket with code {input.Code} has already been added";
                throw new ConflictException(message)
                {
                    ErrorCode = ErrorCodes.AlreadyExist
                };
            }

            var ticket = mapper.Map(input, new Ticket());
            ticket.StrategyId = input.StrategyId;
            strategy.Tickets.Add(ticket);
            
            repository.Update(strategy);
            await unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
