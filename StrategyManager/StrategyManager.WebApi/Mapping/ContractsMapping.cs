using AutoMapper;

namespace StrategyManager.WebAPI.Mapping
{
    /// <summary>
    /// Auto mapper types mapping of Contracts
    /// </summary>
    public class ContractsMapping : Profile
    {
        /// <summary>
        /// Map contracts
        /// </summary>
        public ContractsMapping()
        {
            //Tickets
            CreateMap<Contracts.Jobs.Tickets.AddTicketRequest, Core.Models.Handlers.Jobs.Tickets.AddTicketInput>();
            CreateMap<Contracts.Jobs.Tickets.RemoveTicketRequest, Core.Models.Handlers.Jobs.Tickets.AddTicketInput>();
            CreateMap<Core.Models.DTOs.TicketDTO, Contracts.Jobs.Tickets.TicketViewModel>();

            //Jobs
            CreateMap<Core.Models.DTOs.StrategyDTO, Contracts.Jobs.StrategyViewModel>();
            CreateMap<Core.Models.Handlers.Jobs.GetJobsOutput, Contracts.Jobs.GetJobsResponse>();
            CreateMap<Core.Models.Handlers.Jobs.GetActiveJobsOutput, Contracts.Jobs.GetActiveJobsResponse>();
            CreateMap<Contracts.Jobs.StopJobRequest, Core.Models.Handlers.Jobs.StopJobInput>();
            CreateMap<Contracts.Jobs.StartJobRequest, Core.Models.Handlers.Jobs.StartJobInput>();
        }
    }
}
