using AutoMapper;
using Microsoft.Extensions.Logging;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Strategies.Turtles.Abstractions;
using TradingAPI.API.Services;
using TradingAPI.Contracts.Services.Instruments;
using TradingAPI.Contracts.Services.OrderManager;
using TradingAPI.Contracts.Services.OrderManager.Orders;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class OrderHandler : IOrderHandler
    {
        public StrategyStatus Status { get; private set; }
        public event EventHandler<OrderHandlerEventArgs>? OrderCreated;
        public event EventHandler<OrderHandlerEventArgs>? OrderRejected;
        public event EventHandler<OrderHandlerEventArgs>? OrderCancelled;
        public event EventHandler<OrderHandlerEventArgs>? OrderFilled;
        public event EventHandler<NewStatusEventArgs>? OnStatusChange;
        public EventHandler<OrderStateChangedEventArgs>? OrderStateChanged;

        private string StrategyId { get; set; } = String.Empty;
        private readonly IOrderManager orderManager;
        private readonly IMapper mapper;
        private readonly ILogger<OrderHandler> logger;
        private bool disposed;

        public OrderHandler(
            IOrderManager orderManager,
            IMapper mapper,
            ILogger<OrderHandler> logger)
        {
            this.orderManager = orderManager;
            this.mapper = mapper;
            this.logger = logger;
            orderManager.OrderStateChanged += OrderStateChanged;
            OnStatusChange += OrderHandler_OnStatusChange;
        }

        /// <summary>
        /// Idempotent order handler
        /// </summary>
        public void HandleOrder(OrderHandlerInput input)
        {
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Starting));
            
            OrderStateChanged += OrderManager_OrderStateChanged;
            StrategyId = input.StrategyId;

            var exchangeOrder = this.orderManager.GetOrderById(input.Order.Id);
            if (exchangeOrder == null) RegisterOrder(input);
            else RaiseEventWithOrderState((Order)exchangeOrder);

            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Running));
        }

        private void OrderManager_OrderStateChanged(object? sender, OrderStateChangedEventArgs e)
        {
            LogInformation($"Order {e.Order.Id} status changed to {e.OrderStatus}", e.Order);

            var orderDTO = mapper.Map<Models.DTOs.Strategies.OrderDTO>(e.Order);
            var args = new OrderHandlerEventArgs(orderDTO);
            if (e.OrderStatus == OrderStatus.Active && OrderCreated != null) OrderCreated(this, args);
            else if (e.OrderStatus == OrderStatus.Cancelled && OrderCancelled != null) OrderCancelled(this, args);
            else if (e.OrderStatus == OrderStatus.Filled && OrderFilled != null) OrderFilled(this, args);
            else if (e.OrderStatus == OrderStatus.Rejected && OrderRejected != null) OrderRejected(this, args);
            else
            {
                var message = $"Order status {e.OrderStatus} is not expected";
                throw new ArgumentOutOfRangeException(message);
            }
        }

        private void RaiseEventWithOrderState(Order order)
        {
            if (!Enum.TryParse(order.OrderStatus.ToString(), out OrderStatus orderStatus))
            {
                var message = $"Not expected value {order.OrderStatus}";
                throw new ArgumentOutOfRangeException(nameof(order.OrderStatus), message);
            };
            var args = new OrderStateChangedEventArgs(orderStatus, order);
            OrderManager_OrderStateChanged(this, args);
        }

        private void RegisterOrder(OrderHandlerInput input)
        {
            var instrument = new Instrument
            {
                Code = input.Order.InstrumentCode,
            };

            var order = mapper.Map<Order>(input.Order);
            order.OrderType = TradingAPI.Contracts.Services.OrderManager.Orders.OrderType.Limit;
            order.Instrument = instrument;

            orderManager.RegisterOrder(order);
        }

        public void Stop()
        {
            if (Status == StrategyStatus.Stopping && Status == StrategyStatus.Stopped) return;
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopping));
            OrderStateChanged = null;
            if (OnStatusChange != null) OnStatusChange(this, new NewStatusEventArgs(StrategyStatus.Stopped));
        }

        private void OrderHandler_OnStatusChange(object? sender, NewStatusEventArgs e)
        {
            LogInformation($"New status {e.Status}");
        }

        private void LogInformation(string message, params object[] args)
        {
            var logMessage = $"StrategyId {StrategyId}, step {nameof(OrderHandler)}: ";
            if (args.Any()) logger.LogInformation(logMessage + message, args);
            else logger.LogInformation(logMessage + message, args);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                // TODO: dispose managed state (managed objects (resources)).
                Stop();
                orderManager.OrderStateChanged -= OrderStateChanged;
                OnStatusChange -= OrderHandler_OnStatusChange;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            disposed = true;
        }

        ~OrderHandler() => Dispose(false);
    }
}
