using System;
using System.Threading.Tasks;
using AFI.Registration.Services.Models;
using FluentValidation;

namespace AFI.Registration.Services
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly IValidator<RegistrationRequest> _registrationRequestValidator;

        public CustomerRegistrationService(IValidator<RegistrationRequest> registrationRequestValidator)
        {
            _registrationRequestValidator = registrationRequestValidator;
        }

        public async Task Register(RegistrationRequest registrationRequest)
        {
            var validationResult = await _registrationRequestValidator.ValidateAsync(registrationRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            throw new NotImplementedException();
        }
    }
}
