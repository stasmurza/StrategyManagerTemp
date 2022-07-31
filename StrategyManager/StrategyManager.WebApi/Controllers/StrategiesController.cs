using AutoMapper;
using StrategyManager.Contracts;
using StrategyManager.Contracts.Jobs;
using StrategyManager.Contracts.Jobs.Tickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        /// Returns all jobs.
        /// </summary>
        /// <returns>Jobs <see cref="GetJobsResponse"/></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetJobsResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetJobsAsync()
        {
            var domainOutput = await mediator.Send(new GetJobsInput());
            return Ok(mapper.Map<GetJobsResponse>(domainOutput));
        }

        /// <summary>
        /// Returns all active jobs.
        /// </summary>
        /// <returns>Active jobs <see cref="GetActiveJobsResponse"/></returns>
        [HttpGet("active-jobs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetJobsResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetActiveJobsAsync()
        {
            var domainOutput = await mediator.Send(new GetActiveJobsInput());
            return Ok(mapper.Map<GetActiveJobsResponse>(domainOutput));
        }

        /// <summary>
        /// Add ticket to job
        /// </summary>
        /// <param name="jobId">Id of job</param>
        /// <param name="request">Data of a ticket <see cref="AddTicketRequest"/></param>
        /// <returns></returns>
        [HttpPost("{jobId}/tickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> AddTicketAsync([FromRoute] string jobId, AddTicketRequest request)
        {
            var input = mapper.Map<AddTicketInput>(request);
            input.JobId = jobId;
            await mediator.Send(input);

            return Ok();
        }

        /// <summary>
        /// Remove ticket from job
        /// </summary>
        /// <param name="jobId">Id of job</param>
        /// <param name="request">Data of a ticket <see cref="RemoveTicketRequest"/></param>
        /// <returns></returns>
        [HttpDelete("{jobId}/tickets")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RemoveTicketAsync([FromRoute] string jobId, RemoveTicketRequest request)
        {
            var input = mapper.Map<RemoveTicketInput>(request);
            input.JobId = jobId;
            await mediator.Send(input);

            return NoContent();
        }

        /// <summary>
        /// Run new job
        /// </summary>
        /// <param name="request">Data of a job <see cref="StartJobRequest"/></param>
        /// <returns></returns>
        [HttpPost("active-jobs")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RunNewJobAsync(StartJobRequest request)
        {
            var input = mapper.Map<StartJobInput>(request);
            await mediator.Send(input);

            return NoContent();
        }

        /// <summary>
        /// Stop job
        /// </summary>
        /// <param name="request">Data of a job <see cref="StopJobRequest"/></param>
        /// <returns></returns>
        [HttpDelete("active-jobs")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> StopJobAsync(StopJobRequest request)
        {
            var input = mapper.Map<StopJobInput>(request);
            await mediator.Send(input);

            return NoContent();
        }
    }
}
