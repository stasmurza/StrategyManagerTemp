namespace StrategyManager.Core.Models.Store.Events
{
    public class Event : Entity
    {
        public EntityType EntityType { get; set; }
        public string EntityId { get; set; } = String.Empty;
        public string EventType { get; set; } = String.Empty;
        public string EventData { get; set; } = String.Empty;
    }
}
