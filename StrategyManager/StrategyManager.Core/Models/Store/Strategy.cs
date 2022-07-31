using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StrategyManager.Core.Models.Store
{
    public class Strategy : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public bool StartWithHost { get; set; } = false;
        public IList<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
