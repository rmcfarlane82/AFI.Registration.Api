using System.Threading.Tasks;
using AFI.Registration.Data.Entities;
using AFI.Registration.Services.Models;

namespace AFI.Registration.Services.Interfaces
{
    public interface ICustomerRegistrationService
    {
        Task<Customer> Register(RegistrationRequest registrationRequest);
    }
}