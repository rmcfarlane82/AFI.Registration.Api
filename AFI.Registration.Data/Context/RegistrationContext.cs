using AFI.Registration.Data.Entities;
using AFI.Registration.Data.TypeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace AFI.Registration.Data.Context
{
    public class RegistrationContext : DbContext
    {
        public RegistrationContext(DbContextOptions<RegistrationContext> options): base(options)
        {
            
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerTypeConfiguration());
        }
    }
}
