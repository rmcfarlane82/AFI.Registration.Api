using System;
using System.Threading.Tasks;
using AFI.Registration.Services.Models;
using AFI.Test.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace AFI.Registration.Services.Tests
{
    public class CustomerRegistrationServiceTests
    {
        [Theory, AutoNSubstituteData]
        public void When_registration_request_is_not_valid_THEN_validation_exception_is_thrown(
            RegistrationRequest registrationRequest,
            CustomerRegistrationService sut
            )
        {
            Func<Task> act = () => sut.Register(registrationRequest);

            act.Should().Throw<ValidationException>();
        }
    }
}
