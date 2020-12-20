using System;
using Microsoft.EntityFrameworkCore;

namespace AFI.Registration.Data.Tests.IntegrationTests
{
    public class BaseTestClass<TContext> where TContext : DbContext
    {
        private readonly DbContextOptionsBuilder<TContext> _contextOptionsBuilder;

        public BaseTestClass()
        {
            _contextOptionsBuilder = new DbContextOptionsBuilder<TContext>();

            _contextOptionsBuilder.UseInMemoryDatabase("AFI.Registration.Data.Tests.Database");

            using var context = GetContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public TContext GetContext()
        {
            return (TContext) Activator.CreateInstance(typeof(TContext), _contextOptionsBuilder.Options);
        }
    }
}
