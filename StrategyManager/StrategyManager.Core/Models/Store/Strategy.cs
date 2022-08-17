namespace StrategyManager.Core.Models.Store
{
    public class Strategy : Entity
    {
        public string Code { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public bool StartWithHost { get; set; } = false;
        public IList<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
