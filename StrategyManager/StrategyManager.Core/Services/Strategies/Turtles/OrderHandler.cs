using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Strategies.Turtles.Abstractions;
using TradingAPI.API.Services;
using TradingAPI.Contracts;
using TradingAPI.Contracts.Services.Instruments;
using TradingAPI.Contracts.Services.OrderManager;
using TradingAPI.Contracts.Services.OrderManager.Orders;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class OrderHandler : IOrderHandler
    {
        public event EventHandler<OrderHandlerEventArgs>? OrderCreated;
        public event EventHandler<OrderHandlerEventArgs>? OrderRejected;
        public event EventHandler<OrderHandlerEventArgs>? OrderCancelled;
        public event EventHandler<OrderHandlerEventArgs>? OrderFilled;
        public event EventHandler<EventArgs>? Started;
        public event EventHandler<EventArgs>? Stopped;
        public EventHandler<OrderStateChangedEventArgs>? OrderStateChanged;

        private readonly IOrderManager orderManager;

        public OrderHandler(IOrderManager orderManager)
        {
            this.orderManager = orderManager;
            orderManager.OrderStateChanged += OrderStateChanged;
        }

        /// <summary>
        /// Idempotent order handler
        /// Transmit order to exchange
        /// New order will be created, with subscription to events of order
        /// </summary>
        public void HandleOrder(OrderHandlerInput input)
        {
            OrderStateChanged += OrderManager_OrderStateChanged;

            var exchangeOrder = this.orderManager.GetOrderById(input.OrderId);
            if (exchangeOrder is not null)
            {
                var order = (Order)exchangeOrder;
                if (!Enum.TryParse(order.OrderStatus.ToString(), out OrderStatus orderStatus))
                {
                    var message = $"Not expected value {order.OrderStatus}";
                    throw new ArgumentOutOfRangeException(nameof(order.OrderStatus), message);
                };

                var args = new OrderStateChangedEventArgs
                {
                    OrderStatus = orderStatus
                };
                OrderManager_OrderStateChanged(this, args);
            }
            else RegisterOrder(input);
        }

        private void OrderManager_OrderStateChanged(object? sender, OrderStateChangedEventArgs e)
        {
            if (e.OrderStatus == OrderStatus.Active && OrderCreated != null)
            {
                OrderCreated(this, new OrderHandlerEventArgs());
            }
            else if (e.OrderStatus == OrderStatus.Cancelled && OrderCancelled != null)
            {
                OrderCancelled(this, new OrderHandlerEventArgs());
            }
            else if (e.OrderStatus == OrderStatus.Filled && OrderFilled != null)
            {
                OrderFilled(this, new OrderHandlerEventArgs());
            }
            else if (e.OrderStatus == OrderStatus.Rejected && OrderRejected != null)
            {
                OrderRejected(this, new OrderHandlerEventArgs());
            }
            else
            {
                var message = $"Order status {e.OrderStatus} is not expected";
                throw new ArgumentOutOfRangeException(message);
            }
        }

        private void RegisterOrder(OrderHandlerInput input)
        {
            var instrument = new Instrument
            {
                Code = input.InstrumentCode,
            };

            if (!Enum.TryParse(input.Direction.ToString(), out Direction direction))
            {
                var message = $"Not expected value {input.Direction}";
                throw new ArgumentOutOfRangeException(nameof(input.Direction), message);
            };

            var order = new Order
            {
                Id = input.OrderId,
                OrderType = OrderType.Limit,
                Instrument = instrument,
                Volume = input.Volume,
                Price = input.Price,
                Direction = direction,

            };

            orderManager.RegisterOrder(order);
        }

        public void Stop()
        {
            OrderStateChanged = null;
        }
    }
}
