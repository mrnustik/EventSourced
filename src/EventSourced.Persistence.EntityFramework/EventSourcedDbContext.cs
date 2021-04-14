using EventSourced.Persistence.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourced.Persistence.EntityFramework
{
    public class EventSourcedDbContext : DbContext
    {
        public DbSet<DomainEventEntity> Events { get; set; } = null!;
        
        public EventSourcedDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}