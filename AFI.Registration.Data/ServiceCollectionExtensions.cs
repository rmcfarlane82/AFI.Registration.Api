using AFI.Registration.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AFI.Registration.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterCustomerRegistrationContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RegistrationContext>(
                (serviceProvider, optionsBuilder) =>
                {

                    optionsBuilder.UseSqlServer(connectionString);
                });

            services.AddScoped<DbContext, RegistrationContext>();
            services.AddTransient<IRepository, Repository>();
        }
    }
}
