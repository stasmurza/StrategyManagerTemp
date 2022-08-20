namespace StrategyManager.Core.Models.Services.Strategies
{
    public class NewStatusEventArgs : EventArgs
    {
        public StrategyStatus Status { get; set; }

        public NewStatusEventArgs() { }

        public NewStatusEventArgs(StrategyStatus status) => Status = status;
    }
}
