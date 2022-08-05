using AutoMapper;

namespace StrategyManager.Core.Mapping
{
    public class EntityMapping : Profile
    {
        public EntityMapping()
        {
            //Tickets
            CreateMap<Models.Handlers.Strategies.Tickets.AddTicketInput, Models.Store.Ticket>();
            CreateMap<Models.Store.Ticket, StrategyManager.Core.Models.DTOs.Reports.TicketDTO>();
            CreateMap<Models.Store.Ticket, StrategyManager.Core.Models.DTOs.Strategies.TicketDTO>();

            //Jobs
            CreateMap<Models.Store.Strategy, StrategyManager.Core.Models.DTOs.Reports.StrategyDTO>();
            CreateMap<Models.Store.Strategy, StrategyManager.Core.Models.DTOs.Strategies.StrategyDTO>();
        }
    }
}
