using AutoMapper;
using StrategyManager.Core.Models.DTOs.Strategies;

namespace StrategyManager.Core.Mapping
{
    public class EntityMapping : Profile
    {
        public EntityMapping()
        {
            //Tickets
            CreateMap<Models.Handlers.Strategies.Tickets.AddTicketInput, Models.Store.Ticket>();
            CreateMap<Models.Store.Ticket, TicketDTO>();

            //Jobs
            CreateMap<Models.Store.Strategy, StrategyDTO>();
        }
    }
}
