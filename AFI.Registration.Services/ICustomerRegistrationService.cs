using System.Threading.Tasks;
using AFI.Registration.Services.Models;

namespace AFI.Registration.Services
{
    public interface ICustomerRegistrationService
    {
        Task Register(RegistrationRequest registrationRequest);
    }
}