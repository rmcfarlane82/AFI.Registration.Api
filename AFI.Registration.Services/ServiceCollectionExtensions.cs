using AFI.Registration.Services.Interfaces;
using AFI.Registration.Services.Models;
using AFI.Registration.Services.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AFI.Registration.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterCustomerRegistrationServices(this IServiceCollection services)
        {
            services.AddTransient<ICustomerRegistrationService, CustomerRegistrationService>();
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IValidator<RegistrationRequest>, RegistrationRequestValidator>();
        }
    }
}
