using StrategyManager.Core.Models.Services.Strategies;
using StrategyManager.Core.Models.Services.Strategies.Turtles;
using StrategyManager.Core.Models.Store.Events;
using StrategyManager.Core.Repositories.Abstractions;
using StrategyManager.Core.Services.Abstractions.Strategies;
using System.Text.Json;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    //use event store
    public class TurtlesSaga
    {
        private String Id { get; set; }
        public TurtlesSagaStep SagaStep { get; private set; }
        public StrategyStatus Status { get; private set; }
        public event EventHandler<EventArgs>? StatusChanged;
        private IStrategyStateProvider StateProvider { get; set; }
        private IEntrySignalListener EntrySignalListener { get; set; }
        private IPendingOrderCreator EntryOrderCreator { get; set; }
        private IIdempotentCommand PerformEntryOrder { get; set; }
        private IIdempotentCommand ListenExitSignal { get; set; }
        private IIdempotentCommand CreatePendingExitOrder { get; set; }
        private IIdempotentCommand PerformExitOrder { get; set; }
        private EventHandler<EventArgs>? StepStartHandler;
        private EventHandler<EventArgs>? StepErrorHandler;
        private IRepository<Event> eventRepository;

        public TurtlesSaga(
            IRepository<Event> eventRepository,
            IStrategyStateProvider stateProvider,
            IEntrySignalListener entrySignalListener,
            IPendingOrderCreator entryOrderCreator,
            IIdempotentCommand performEntryOrder,
            IIdempotentCommand listenExitSignal,
            IIdempotentCommand createExitPendingOrder,
            IIdempotentCommand performExitOrder)
        {
            this.eventRepository = eventRepository;
            StateProvider = stateProvider;
            EntrySignalListener = entrySignalListener;
            EntryOrderCreator = entryOrderCreator;
            PerformEntryOrder = performEntryOrder;
            ListenExitSignal = listenExitSignal;
            CreatePendingExitOrder = createExitPendingOrder;
            PerformExitOrder = performExitOrder;

            EntrySignalListener.EntrySignal += ListenEntrySignal_EntrySignal;
        }

        private void ListenEntrySignal_EntrySignal(object? sender, EntrySignalEventArgs e)
        {
            SagaStep = TurtlesSagaStep.CreatingEntryPendingOrder;
            var input = new PendingOrderInput();
            EntryOrderCreator.CreatePendingOrder(input);
        }

        private async Task Run(string ticketCode)
        {
            var lastEvent = await eventRepository.FirstOrDefaultAsync(i => i.EntityId == Id, "Desc");
            if (lastEvent is null)
            {
                var input = new EntrySignalInput();
                EntrySignalListener.Run(input);
            }
            else if (lastEvent.EventType == "EntrySignal")
            {
                var args = JsonSerializer.Deserialize<EntrySignalEventArgs>(lastEvent.EventData);
                var message = "";
                if (args == null) throw new ArgumentNullException(message);
                ListenEntrySignal_EntrySignal(EntrySignalListener, args);
            }
        }

        public void Start(string ticketCode)
        {
            SetStatus(StrategyStatus.Starting);
            var jsonString = StateProvider.GetState();
            var step = JsonSerializer.Deserialize<TurtlesSagaStep>(jsonString);
            var sagaStep = GetSagaStep(step);
            CreateChain();
            sagaStep.Run();
            SetStatus(StrategyStatus.Running);
        }

        public void Stop()
        {
            SetStatus(StrategyStatus.Stopping);

            EntrySignalListener.Stop();
            EntryPendingOrder.Stop();
            PerformEntryOrder.Stop();
            ListenExitSignal.Stop();
            CreatePendingExitOrder.Stop();
            PerformExitOrder.Stop();
            SetStatus(StrategyStatus.Stopped);
        }

        private void CreateChain()
        {
            EntrySignalListener.Next = EntryPendingOrder;
            EntryPendingOrder.Next = PerformEntryOrder;
            PerformEntryOrder.Next = ListenExitSignal;
            ListenExitSignal.Next = CreatePendingExitOrder;
            CreatePendingExitOrder.Next = PerformExitOrder;
            PerformExitOrder.Next = EntrySignalListener;
        }

        private void SetStatus(StrategyStatus status)
        {
            Status = status;
            if (StatusChanged != null) StatusChanged(this, EventArgs.Empty);
        }

        private IIdempotentCommand GetSagaStep(TurtlesSagaStep step)
        {
            var result = step switch
            {
                TurtlesSagaStep.ListeningEntrySignal => EntrySignalListener,
                TurtlesSagaStep.CreatingEntryPendingOrder => EntryPendingOrder,
                TurtlesSagaStep.PerformingEntryOrder => PerformEntryOrder,
                TurtlesSagaStep.ListeningExitSignal => ListenExitSignal,
                TurtlesSagaStep.CreatingExitPendingOrder => CreatePendingExitOrder,
                TurtlesSagaStep.PerformingExitOrder => PerformExitOrder,
                _ => throw new ArgumentOutOfRangeException(nameof(step), $"Not expected state value: {step}"),
            };

            return result;
        }

        private void OnStepStart(Object? o, EventArgs a)
        {
            int index = (int)SagaStep;
            if (SagaStep == TurtlesSagaStep.PerformingExitOrder) index = 0;
            else index++;
            SagaStep = (TurtlesSagaStep)index;
        }

        private void OnStepError(Object? o, EventArgs a)
        {
            Status = StrategyStatus.Error;
        }
    }

    public enum TurtlesSagaStep
    {
        ListeningEntrySignal,
        CreatingEntryPendingOrder,
        PerformingEntryOrder,
        ListeningExitSignal,
        CreatingExitPendingOrder,
        PerformingExitOrder,
    }

    public enum TurtlesSagaState
    {
        Starting,
        Running,
        Stopping,
        Stopped,
        Error,
    }
}
