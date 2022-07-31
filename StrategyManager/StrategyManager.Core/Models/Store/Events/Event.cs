using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StrategyManager.Core.Models.Store.Events
{
    public class Event : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        public string EventType { get; set; } = String.Empty;
        public string EntityType { get; set; } = String.Empty;
        public string EntityId { get; set; } = String.Empty;
        public string EventData { get; set; } = String.Empty;
        public bool Published { get; set; } = false;
    }
}
