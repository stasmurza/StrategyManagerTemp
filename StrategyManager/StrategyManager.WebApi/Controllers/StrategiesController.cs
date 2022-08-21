using AutoMapper;
using StrategyManager.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StrategyManager.Contracts.Strategies;
using StrategyManager.Core.Models.Handlers.Strategies;
using StrategyManager.Contracts.Strategies.Tickets;
using StrategyManager.Core.Models.Handlers.Strategies.Tickets;

namespace StrategyManager.WebApi.Controllers
{
    /// <summary>
    /// Controller for strategies management.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class StrategiesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public StrategiesController(
            IMediator mediator,
            IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns all strategies.
        /// </summary>
        /// <returns>Strategies <see cref="GetStrategiesResponse"/></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetStrategiesResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetStrategiesAsync()
        {
            var domainOutput = await mediator.Send(new GetStrategiesInput());
            return Ok(mapper.Map<GetStrategiesResponse>(domainOutput));
        }

        /// <summary>
        /// Add ticket to strategy
        /// </summary>
        /// <param name="strategyId">Id of strategy</param>
        /// <param name="request">Data of a ticket <see cref="AddTicketRequest"/></param>
        /// <returns></returns>
        [HttpPost("{strategyId}/tickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> AddTicketAsync([FromRoute] int strategyId, AddTicketRequest request)
        {
            var input = mapper.Map<AddTicketInput>(request);
            input.StrategyId = strategyId;
            await mediator.Send(input);

            return Ok();
        }

        /// <summary>
        /// Remove ticket from strategy
        /// </summary>
        /// <param name="strategyId">Id of strategy</param>
        /// <param name="request">Data of a ticket <see cref="RemoveTicketRequest"/></param>
        /// <returns></returns>
        [HttpDelete("{strategyId}/tickets")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RemoveTicketAsync([FromRoute] int strategyId, RemoveTicketRequest request)
        {
            var input = mapper.Map<RemoveTicketInput>(request);
            input.StrategyId = strategyId;
            await mediator.Send(input);

            return NoContent();
        }

        /// <summary>
        /// Run new strategy
        /// </summary>
        /// <param name="strategyId">Id of strategy</param>
        /// <returns></returns>
        [HttpPost("active-strategies/{strategyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RunStrategyAsync(int strategyId)
        {
            var input = new RunStrategyInput
            {
                Id = strategyId
            };

            await mediator.Send(input);

            return NoContent();
        }

        /// <summary>
        /// Stop strategy
        /// </summary>
        /// <param name="strategyId">Id of strategy</param>
        /// <returns></returns>
        [HttpDelete("active-strategies/{strategyId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> StopStrategyAsync(int strategyId)
        {
            var input = new StopStrategyInput
            {
                Id = strategyId
            };

            await mediator.Send(input);

            return NoContent();
        }
    }
}
