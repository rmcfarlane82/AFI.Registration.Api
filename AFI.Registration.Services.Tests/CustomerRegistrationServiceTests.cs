using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
