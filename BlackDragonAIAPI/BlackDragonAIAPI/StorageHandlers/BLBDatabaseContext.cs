using BlackDragonAIAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlackDragonAIAPI.StorageHandlers
{
    public class BLBDatabaseContext : DbContext
    {
        public BLBDatabaseContext(DbContextOptions<BLBDatabaseContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<CommandDetails> Commands { get; set; }
        public DbSet<TimedMessage> TimedMessages { get; set; }
        public DbSet<WebhookSubscriber> WebhookSubscribers { get; set; }
        public DbSet<DeathCount> DeathCounts { get; set; }
        public DbSet<StreamPlanning> StreamPlannings { get; set; }
        public DbSet<BannedTerm> BannedTerms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimedMessage>().Property("Guid").HasColumnType("binary(16)");
            modelBuilder.Entity<WebhookSubscriber>().Property("Guid").HasColumnType("binary(16)");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
