using Microsoft.EntityFrameworkCore;
using StrategyManager.Core.Models.Store;
using StrategyManager.Core.Models.Store.Events;

namespace StrategyManager.Data
{
    public class StrategyManagerDbContext : DbContext
    {
        public StrategyManagerDbContext(DbContextOptions<StrategyManagerDbContext> options) : base(options) { }

        public DbSet<Strategy>? Strategies { get; set; }
        public DbSet<Event>? Events { get; set; }
        public DbSet<Order>? Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Order>()
                .HasIndex(u => u.Guid)
                    .IsUnique();

            modelBuilder
                .Entity<Trade>()
                .HasOne<Order>(s => s.Order)
                .WithMany(g => g.Trades)
                .HasForeignKey(s => s.OrderId);
        }
    }
}
