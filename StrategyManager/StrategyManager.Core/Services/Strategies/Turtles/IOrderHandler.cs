﻿using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Services.Abstractions.Strategies;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public interface IOrderHandler : IIdempotentCommand
    {
        /// <summary>
        /// Idempotent order handler
        /// Transmit order to exchange
        /// If such order exist, event with actual state will be generated
        /// Otherwise new order will be created, with subscription to events of order
        /// </summary>
        public void HandleOrder(OrderHandlerInput input);

        public event EventHandler<OrderHandlerEventArgs>? OrderCreated;
        public event EventHandler<OrderHandlerEventArgs>? OrderRejected;
        public event EventHandler<OrderHandlerEventArgs>? OrderCancelled;
        public event EventHandler<OrderHandlerEventArgs>? OrderFilled;
    }
}
