using Microsoft.EntityFrameworkCore;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Models.Store.Events;

namespace StrategyManager.Data
{
    public class StrategyManagerDbContext : DbContext
    {
        public StrategyManagerDbContext(DbContextOptions<StrategyManagerDbContext> options) : base(options) { }

        public DbSet<Event>? Events { get; set; }
        public DbSet<Order>? Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Ticket>()
                .HasOne<Order>(s => s.Order)
                .WithMany(g => g.Tickets)
                .HasForeignKey(s => s.OrderId);
        }
    }
}
