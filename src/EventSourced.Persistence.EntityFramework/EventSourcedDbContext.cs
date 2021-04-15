using EventSourced.Persistence.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourced.Persistence.EntityFramework
{
    public class EventSourcedDbContext : DbContext
    {
        public DbSet<DomainEventEntity> Events { get; set; } = null!;
        public DbSet<AggregateSnapshotEntity> AggregateSnapshots { get; set; } = null!;
        public DbSet<TypeBasedProjectionEntity> TypeBasedProjections { get; set; } = null!;
        public DbSet<AggregateBasedProjectionEntity> AggregateBasedProjections { get; set; } = null!;

        public EventSourcedDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AggregateBasedProjectionEntity>()
                        .HasKey(p => new {p.AggregateRootId, p.SerializedProjectionType});
        }
    }
}