using AutoMapper;
using StrategyManager.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StrategyManager.Core.Models.Handlers.StrategiesReport;
using StrategyManager.Contracts.StrategiesReport;

namespace StrategyManager.WebApi.Controllers
{
    /// <summary>
    /// Controller for reports of strategies.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public ReportsController(
            IMediator mediator,
            IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Returns all active strategies.
        /// </summary>
        /// <returns>Active strategies <see cref="ActiveStrategiesReportResponse"/></returns>
        [HttpGet("active-strategies")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActiveStrategiesReportResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ActiveStrategiesReportAsync()
        {
            var domainOutput = await mediator.Send(new ActiveStrategiesReportInput());
            return Ok(mapper.Map<ActiveStrategiesReportResponse>(domainOutput));
        }
    }
}
