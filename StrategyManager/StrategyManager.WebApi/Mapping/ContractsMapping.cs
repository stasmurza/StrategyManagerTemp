using AutoMapper;

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
            CreateMap<Contracts.Strategies.Tickets.RemoveTicketRequest, Core.Models.Handlers.Strategies.Tickets.RemoveTicketInput>();
            CreateMap<StrategyManager.Core.Models.DTOs.Strategies.TicketDTO, Contracts.Strategies.Tickets.TicketViewModel>();

            //Strategies
            CreateMap<StrategyManager.Core.Models.DTOs.Strategies.StrategyDTO, Contracts.Strategies.StrategyViewModel>();
            CreateMap<Core.Models.Handlers.Strategies.GetStrategiesOutput, Contracts.Strategies.GetStrategiesResponse>();
            CreateMap<Contracts.Strategies.StopStrategyRequest, Core.Models.Handlers.Strategies.StopStrategyInput>();
            CreateMap<Contracts.Strategies.RunStrategyRequest, Core.Models.Handlers.Strategies.RunStrategyInput>();

            //Reports
            CreateMap<StrategyManager.Core.Models.DTOs.Reports.TicketDTO, Contracts.StrategiesReport.TicketReportViewModel>();
            CreateMap<StrategyManager.Core.Models.DTOs.Reports.StrategyDTO, Contracts.StrategiesReport.StrategyReportViewModel>();
            CreateMap<Core.Models.Handlers.StrategiesReport.ActiveStrategiesReportOutput, Contracts.StrategiesReport.ActiveStrategiesReportResponse>();
        }
    }
}
