using System;
using EventSourced.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace EventSourced.Tests.Persistence.EntityFramework
{
    public static class TestDbContextFactory
    {
        public static EventSourcedDbContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventSourcedDbContext>();
            optionsBuilder.UseInMemoryDatabase(DateTime.Now.Ticks.ToString());
            return new EventSourcedDbContext(optionsBuilder.Options);
        }
    }
}