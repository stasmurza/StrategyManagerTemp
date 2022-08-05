using AutoMapper;
using StrategyManager.Core.Models.DTOs.Strategies;

namespace StrategyManager.WebAPI.Mapping
{
    /// <summary>
    /// Auto mapper types mapping
    /// </summary>
    public class ContractsMapping : Profile
    {
        /// <summary>
        /// Map contracts
        /// </summary>
        public ContractsMapping()
        {
            //Tickets
            CreateMap<Contracts.Strategies.Tickets.AddTicketRequest, Core.Models.Handlers.Strategies.Tickets.AddTicketInput>();
            CreateMap<Contracts.Strategies.Tickets.RemoveTicketRequest, Core.Models.Handlers.Strategies.Tickets.AddTicketInput>();
            CreateMap<TicketDTO, Contracts.Strategies.Tickets.TicketViewModel>();

            //Strategies
            CreateMap<StrategyDTO, Contracts.Strategies.StrategyViewModel>();
            CreateMap<Core.Models.Handlers.Strategies.GetStrategiesOutput, Contracts.Strategies.GetStrategiesResponse>();
            CreateMap<Contracts.Strategies.StopStrategyRequest, Core.Models.Handlers.Strategies.StopStrategyInput>();
            CreateMap<Contracts.Strategies.RunStrategyRequest, Core.Models.Handlers.Strategies.RunStrategyInput>();

            //Reports
            CreateMap<Core.Models.Handlers.StrategiesReport.ActiveStrategiesReportOutput, Contracts.StrategiesReport.ActiveStrategiesReportResponse>();
        }
    }
}
