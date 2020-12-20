using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AFI.Registration.Data;
using AFI.Registration.Data.Entities;
using AFI.Registration.Services.Models;
using AFI.Test.Helpers;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Xunit;

namespace AFI.Registration.Services.Tests
{
    public class CustomerRegistrationServiceTests
    {
        [Theory, AutoNSubstituteData]
        public void When_registration_request_is_not_valid_THEN_validation_exception_is_thrown(
            RegistrationRequest registrationRequest,
            List<ValidationFailure> validationFailures,
            [Frozen]IValidator<RegistrationRequest> registrationRequestValidator,
            CustomerRegistrationService sut
            )
        {
            registrationRequestValidator.ValidateAsync(registrationRequest).Returns(new ValidationResult(validationFailures));

            Func<Task> act = () => sut.Register(registrationRequest);

            act.Should().Throw<ValidationException>();
        }

        [Theory, AutoNSubstituteData]
        public async Task When_registration_request_is_valid_THEN_new_customer_is_sent_to_repository_and_saved(
            RegistrationRequest registrationRequest,
            [Frozen]IRepository repository,
            [Frozen]IValidator<RegistrationRequest> registrationRequestValidator,
            CustomerRegistrationService sut
        )
        {
            registrationRequestValidator.ValidateAsync(registrationRequest).Returns(new ValidationResult());

            repository.SaveChangesAsync().Returns(1);

            await sut.Register(registrationRequest);

            await repository.Received(1).AddAsync(Arg.Is<Customer>(customer =>
                customer.FirstName == registrationRequest.FirstName &&
                customer.LastName == registrationRequest.LastName &&
                customer.DateOfBirth == registrationRequest.DateOfBirth &&
                customer.EmailAddress == registrationRequest.EmailAddress &&
                customer.ReferenceNumber == registrationRequest.ReferenceNumber
            ));

            await repository.Received(1).SaveChangesAsync();
        }

        [Theory, AutoNSubstituteData]
        public void When_repository_has_no_saved_changes_THEN_exception_is_thrown(
            RegistrationRequest registrationRequest,
            [Frozen]IRepository repository,
            [Frozen]IValidator<RegistrationRequest> registrationRequestValidator,
            CustomerRegistrationService sut
        )
        {
            registrationRequestValidator.ValidateAsync(registrationRequest).Returns(new ValidationResult());

            repository.SaveChangesAsync().Returns(0);

            Func<Task> act = () => sut.Register(registrationRequest);

            act.Should().Throw<Exception>().WithMessage("Failed to save customer");
        }

        [Theory, AutoNSubstituteData]
        public async Task When_customer_saves_THEN_customer_is_returned(
            RegistrationRequest registrationRequest,
            [Frozen]IRepository repository,
            [Frozen]IValidator<RegistrationRequest> registrationRequestValidator,
            CustomerRegistrationService sut
        )
        {
            registrationRequestValidator.ValidateAsync(registrationRequest).Returns(new ValidationResult());

            repository.SaveChangesAsync().Returns(1);

            var result = await sut.Register(registrationRequest);

            result.Should().BeOfType<Customer>();
        }
    }
}
