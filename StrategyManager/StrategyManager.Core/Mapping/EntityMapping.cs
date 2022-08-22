using AutoMapper;

namespace StrategyManager.Core.Mapping
{
    public class EntityMapping : Profile
    {
        public EntityMapping()
        {
            //Tickets
            CreateMap<Models.Handlers.Strategies.Tickets.AddTicketInput, Models.Store.Ticket>();
            CreateMap<Models.Store.Ticket, Models.DTOs.Reports.TicketDTO>();
            CreateMap<Models.Store.Ticket, Models.DTOs.Strategies.TicketDTO>();

            //Strategy
            CreateMap<Models.Store.Strategy, Models.DTOs.Reports.StrategyDTO>();
            CreateMap<Models.Store.Strategy, Models.DTOs.Strategies.StrategyDTO>();
            CreateMap<Models.Services.Strategies.Direction, TradingAPI.Contracts.Direction>();
            CreateMap<TradingAPI.Contracts.Direction, Models.Services.Strategies.Direction>();

            //Turtles
            CreateMap<Models.Services.Strategies.Turtles.PendingOrderInput, Models.Store.Order>();
            CreateMap<Models.Store.Order, Models.DTOs.Strategies.OrderDTO>();
            CreateMap<Models.DTOs.Strategies.OrderDTO, TradingAPI.Contracts.Services.OrderManager.Orders.Order>();
            CreateMap<Models.Store.Order, TradingAPI.Contracts.Services.OrderManager.Orders.Order>();
        }
    }
}
