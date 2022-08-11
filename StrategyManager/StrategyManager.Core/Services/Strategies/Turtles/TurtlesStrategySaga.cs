using StrategyManager.Core.Services.Abstractions.Strategies;
using System.Text.Json;

namespace StrategyManager.Core.Services.Strategies.Turtles
{
    public class TurtlesStrategySaga
    {
        public TurtlesSagaState SagaState { get; private set; }
        private IStrategyStateProvider StateProvider { get; set; }
        private IIdempotentStep ListenEntrySignal { get; set; }
        private IIdempotentStep CreateEntryOrder { get; set; }
        private IIdempotentStep ListenEntryOrder { get; set; }
        private IIdempotentStep ListenExitSignal { get; set; }
        private IIdempotentStep CreateExitOrder { get; set; }
        private IIdempotentStep ListenExitOrder { get; set; }
        private EventHandler<EventArgs>? StepStartHandler;
        private EventHandler<EventArgs>? StepErrorHandler;

        public TurtlesStrategySaga(
            IStrategyStateProvider stateProvider,
            IIdempotentStep listenEntrySignal,
            IIdempotentStep createEntryOrder,
            IIdempotentStep listenEntryOrder,
            IIdempotentStep listenExitSignal,
            IIdempotentStep createExitOrder,
            IIdempotentStep listenExitOrder)
        {
            StateProvider = stateProvider;
            ListenEntrySignal = listenEntrySignal;
            CreateEntryOrder = createEntryOrder;
            ListenEntryOrder = listenEntryOrder;
            ListenExitSignal = listenExitSignal;
            CreateExitOrder = createExitOrder;
            ListenExitOrder = listenExitOrder;

            StepStartHandler += OnStepStart;
            ListenEntrySignal.Start += StepStartHandler;
            CreateEntryOrder.Start += StepStartHandler;
            ListenEntryOrder.Start += StepStartHandler;
            ListenExitSignal.Start += StepStartHandler;
            CreateExitOrder.Start += StepStartHandler;
            ListenExitOrder.Start += StepStartHandler;

            StepErrorHandler += OnStepError;
            ListenEntrySignal.Error += StepErrorHandler;
            CreateEntryOrder.Error += StepErrorHandler;
            ListenEntryOrder.Error += StepErrorHandler;
            ListenExitSignal.Error += StepErrorHandler;
            CreateExitOrder.Error += StepErrorHandler;
            ListenExitOrder.Error += StepErrorHandler;
        }

        public void CreateChain()
        {
            ListenEntrySignal.Next = CreateEntryOrder;
            CreateEntryOrder.Next = ListenEntryOrder;
            ListenEntryOrder.Next = ListenExitSignal;
            ListenExitSignal.Next = CreateExitOrder;
            CreateExitOrder.Next = ListenExitOrder;
            ListenExitOrder.Next = ListenEntrySignal;
        }

        public void Start()
        {
            var jsonString = StateProvider.GetState();
            var state = JsonSerializer.Deserialize<TurtlesSagaState>(jsonString);
            if (state == TurtlesSagaState.Error) throw new ArgumentException($"Can not start from state {state}");

            var step = GetActiveStep(state);
            CreateChain();
            step.Run();
        }

        public void Stop()
        {
            ListenEntrySignal.Next = null;
            CreateEntryOrder.Next = null;
            ListenEntryOrder.Next = null;
            ListenExitSignal.Next = null;
            CreateExitOrder.Next = null;
            ListenExitOrder.Next = null;

            ListenEntrySignal.Stop();
            CreateEntryOrder.Stop();
            ListenEntryOrder.Stop();
            ListenExitSignal.Stop();
            CreateExitOrder.Stop();
            ListenExitOrder.Stop();
            SagaState = TurtlesSagaState.Stopped;
        }

        private IIdempotentStep GetActiveStep(TurtlesSagaState state)
        {
            var step = state switch
            {
                TurtlesSagaState.ListeningEntrySignal => ListenEntrySignal,
                TurtlesSagaState.CreatingEntryOrder => CreateEntryOrder,
                TurtlesSagaState.ListeningEntryOrder => ListenEntryOrder,
                TurtlesSagaState.ListeningExitSignal => ListenExitSignal,
                TurtlesSagaState.CreatingExitOrder => CreateExitOrder,
                TurtlesSagaState.ListeningExitOrder => ListenExitOrder,
                TurtlesSagaState.Error => throw new InvalidOperationException($"Not expected state value: {state}"),
                _ => throw new ArgumentOutOfRangeException(nameof(state), $"Not expected state value: {state}"),
            };

            return step;
        }

        private void OnStepStart(Object? o, EventArgs a)
        {
            int index = (int)SagaState;
            if (SagaState == TurtlesSagaState.ListeningExitOrder) index = 0;
            else index++;
            SagaState = (TurtlesSagaState)index;
        }

        private void OnStepError(Object? o, EventArgs a)
        {
            SagaState = TurtlesSagaState.Error;
        }
    }

    public enum TurtlesSagaState
    {
        ListeningEntrySignal,
        CreatingEntryOrder,
        ListeningEntryOrder,
        ListeningExitSignal,
        CreatingExitOrder,
        ListeningExitOrder,
        Error,
        Stopped,
    }
}
