using System;
using System.Threading.Tasks;
using AFI.Registration.Data;
using AFI.Registration.Data.Entities;
using AFI.Registration.Services.Interfaces;
using AFI.Registration.Services.Models;
using FluentValidation;

namespace AFI.Registration.Services
{
    public class CustomerRegistrationService : ICustomerRegistrationService
    {
        private readonly IValidator<RegistrationRequest> _registrationRequestValidator;
        private readonly IRepository _repository;

        public CustomerRegistrationService(
            IValidator<RegistrationRequest> registrationRequestValidator,
            IRepository repository)
        {
            _registrationRequestValidator = registrationRequestValidator;
            _repository = repository;
        }

        public async Task<Customer> Register(RegistrationRequest registrationRequest)
        {
            var validationResult = await _registrationRequestValidator.ValidateAsync(registrationRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var customer = new Customer(
                registrationRequest.FirstName,
                registrationRequest.LastName,
                registrationRequest.ReferenceNumber,
                registrationRequest.DateOfBirth,
                registrationRequest.EmailAddress
            );

            await _repository.AddAsync(customer);

            var saveChanges = await _repository.SaveChangesAsync();

            if (saveChanges == 0)
            {
                throw new Exception("Failed to save customer");
            }

            return customer;
        }
    }
}
