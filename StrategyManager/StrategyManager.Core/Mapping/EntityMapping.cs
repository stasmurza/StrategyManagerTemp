using AutoMapper;

namespace StrategyManager.Core.Mapping
{
    public class EntityMapping : Profile
    {
        public EntityMapping()
        {
            //Tickets
            CreateMap<Models.Handlers.Jobs.Tickets.AddTicketInput, Models.Store.Ticket>();
            CreateMap<Models.Store.Ticket, Models.DTOs.TicketDTO>();

            //Jobs
            CreateMap<Models.Store.Strategy, Models.DTOs.StrategyDTO>();
        }
    }
}
