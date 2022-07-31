using Microsoft.Extensions.Logging;
using StrategyManager.Core.Exceptions;
using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Services.Abstractions;
using StrategyManager.Core.Services.Strategies;
using System.Collections.Concurrent;

namespace StrategyManager.Core.Services
{
    public class StrategyManager : IStrategyManager
    {
        private ConcurrentDictionary<string, (IStrategy strategy, Task task)> strategies;
        private readonly ILogger logger;

        public StrategyManager(ILogger<StrategyManager> logger)
        {
            strategies = new ();
            this.logger = logger;
        }

        public TicketStatus GetTicketStatus(string strategyCode, string ticketCode)
        {
            var key = strategyCode + ticketCode;
            if (!strategies.TryGetValue(key, out (IStrategy, Task) tuple))
            {
                var message = $"Strategy {strategyCode} with ticket {ticketCode} is not found";
                throw new NotFoundException(message);
            }

            var strategy = tuple.Item1;

            return new TicketStatus
            {
                StrategyCode = strategy.Code.ToString(),
                TicketCode = strategy.TicketCode,
                Status = strategy.Status.ToString(),
                LastActive = strategy.LastActive,
            };
        }

        public IEnumerable<TicketStatus> GetTicketStatuses()
        {
            var list = strategies.Select(i => i.Value.strategy).ToList();
            foreach (var strategy in list)
            {
                yield return new TicketStatus
                {
                    StrategyCode = strategy.Code.ToString(),
                    TicketCode = strategy.TicketCode,
                    Status = strategy.Status.ToString(),
                    LastActive = strategy.LastActive,
                };
            };
        }

        public void Start(string strategyCode, string ticketCode)
        {
            var key = strategyCode + ticketCode;
            if (strategies.ContainsKey(key))
            {
                throw new ConflictException("Strategy {strategyCode} with ticket {ticketCode} is running");
            }

            var strategy = new TurtlesStrategy();
            var task = Task.Run(async () => await strategy.StartAsync(new CancellationTokenSource()));
            strategies.TryAdd(key, (strategy, task));

            var continueTask = task.ContinueWith(async (t) =>
            {
                logger.LogError(t?.Exception?.Message);
                await RestartAsync(strategyCode, ticketCode);
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public async Task StopAsync(string strategyCode, string ticketCode)
        {
            var key = strategyCode + ticketCode;
            if (!strategies.TryGetValue(key, out (IStrategy, Task) tuple))
            {
                throw new NotFoundException($"Strategy {strategyCode} with ticket {ticketCode} is not found");
            }

            var strategy = tuple.Item1;
            var task = tuple.Item2;

            await strategy.StopAsync();
            strategy.Dispose();
            task.Dispose();
            strategies.TryRemove(key, out _);
        }

        private async Task RestartAsync(string strategyCode, string ticketCode)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    await StopAsync(strategyCode, ticketCode);
                    Start(strategyCode, ticketCode);
                    break;
                }
                catch (Exception exception)
                {
                    var message = $"Unsuccessfull attempt {i} to restart strategy {strategyCode}, ticket {ticketCode}";
                    logger.LogError(exception, message);
                }
                await Task.Delay(i * 5000);
            }

            //send email
            await NotifyAsync(strategyCode, ticketCode);
        }

        private async Task NotifyAsync(string strategyCode, string ticketCode)
        {

        }
    }
}
