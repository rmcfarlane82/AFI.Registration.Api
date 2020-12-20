using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AFI.Registration.Data.Tests.IntegrationTests
{
    public class BaseTestClass<TContext> where TContext : DbContext
    {
        private readonly DbContextOptionsBuilder<TContext> _contextOptionsBuilder;

        public BaseTestClass()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            _contextOptionsBuilder = new DbContextOptionsBuilder<TContext>();

            _contextOptionsBuilder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=afi_customers_db_integration_test;Trusted_Connection=True;MultipleActiveResultSets=true")
                .UseInternalServiceProvider(serviceProvider);

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
