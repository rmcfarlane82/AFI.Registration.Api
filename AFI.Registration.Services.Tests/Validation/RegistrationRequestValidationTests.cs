using System;
using System.Threading.Tasks;
using AFI.Registration.Services.Interfaces;
using AFI.Registration.Services.Models;
using AFI.Registration.Services.Validation;
using AFI.Test.Helpers;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
using NSubstitute;
using Xunit;

namespace AFI.Registration.Services.Tests.Validation
{
    public class RegistrationRequestValidationTests
    {
        public class RegistrationRequestValidatorTests
        {
            public class FirstNameTests
            {
                [Theory, AutoNSubstituteData]
                public async Task When_first_name_is_between_3_and_50_chars_THEN_no_validation_errors(
                    Fixture fixture,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        FirstName = fixture.Create<string>()
                    };

                    await sut.ShouldNotHaveValidationErrorForAsync(x => x.FirstName, registrationRequest);
                }

                [Theory, AutoNSubstituteData]
                public void When_first_name_is_less_than_3_chars_THEN_contains_validation_errors(
                    Fixture fixture,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        FirstName = string.Join(string.Empty, fixture.CreateMany<char>(2))
                    };

                    sut.ShouldHaveValidationErrorFor(x => x.FirstName, registrationRequest)
                        .WithErrorMessage("First name too short");
                }

                [Theory, AutoNSubstituteData]
                public void When_first_name_is_more_than_50_chars_THEN_contains_validation_errors(
                    Fixture fixture,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        FirstName = string.Join(string.Empty, fixture.CreateMany<char>(51).ToString())
                    };

                    sut.ShouldHaveValidationErrorFor(x => x.FirstName, registrationRequest)
                        .WithErrorMessage("First name too long");
                }
            }

            public class LastNameTests
            {
                [Theory, AutoNSubstituteData]
                public async Task When_last_name_is_between_3_and_50_chars_THEN_no_validation_errors(
                    Fixture fixture,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        LastName = fixture.Create<string>()
                    };

                    await sut.ShouldNotHaveValidationErrorForAsync(x => x.LastName, registrationRequest);
                }

                [Theory, AutoNSubstituteData]
                public void When_last_name_is_less_than_3_chars_THEN_contains_validation_errors(
                    Fixture fixture,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        LastName = string.Join(string.Empty, fixture.CreateMany<char>(2))
                    };

                    sut.ShouldHaveValidationErrorFor(x => x.LastName, registrationRequest)
                        .WithErrorMessage("Last name too short");
                }

                [Theory, AutoNSubstituteData]
                public void When_last_name_is_more_than_50_chars_THEN_contains_validation_errors(
                    Fixture fixture,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        LastName = string.Join(string.Empty, fixture.CreateMany<char>(51).ToString())
                    };

                    sut.ShouldHaveValidationErrorFor(x => x.LastName, registrationRequest)
                        .WithErrorMessage("Last name too long");
                }
            }

            public class ReferenceNumberTests
            {
                [Theory]
                [InlineAutoNSubstituteData("xx-123456")]
                [InlineAutoNSubstituteData("XX-1234567")]
                [InlineAutoNSubstituteData("XX123456")]
                [InlineAutoNSubstituteData("12-123456")]
                [InlineAutoNSubstituteData("123456-XX")]
                [InlineAutoNSubstituteData("XXX-123456")]
                public void When_reference_number_is_not_in_correct_format_THEN_contains_validation_errors(
                    string referenceNumber,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        ReferenceNumber = referenceNumber
                    };

                    sut.ShouldHaveValidationErrorFor(x => x.ReferenceNumber, registrationRequest);
                }

                [Theory]
                [InlineAutoNSubstituteData("XX-123456")]
                [InlineAutoNSubstituteData("AA-123456")]
                [InlineAutoNSubstituteData("AA-768768")]
                public void When_reference_number_is_in_correct_format_THEN_no_validation_errors(
                    string referenceNumber,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        ReferenceNumber = referenceNumber
                    };

                    sut.ShouldNotHaveValidationErrorFor(x => x.ReferenceNumber, registrationRequest);
                }
            }

            public class DateOfBirthTests
            {
                [Theory]
                [InlineAutoNSubstituteData("2000-01-01")]
                [InlineAutoNSubstituteData("1999-01-02")]
                public void When_younger_than_18_THEN_contains_validation_errors(
                    string dateOfBirth,
                    [Frozen] IDateTimeService dateTimeService,
                    RegistrationRequestValidator sut)
                {
                    dateTimeService.Now().Invoke().Returns(new DateTime(2017, 01, 01));

                    var registrationRequest = new RegistrationRequest
                    {
                        DateOfBirth = DateTime.Parse(dateOfBirth)
                    };

                    sut.ShouldHaveValidationErrorFor(x => x.DateOfBirth, registrationRequest)
                        .WithErrorMessage("Too young");
                }

                [Theory]
                [InlineAutoNSubstituteData("2000-01-01")]
                [InlineAutoNSubstituteData("1999-01-01")]
                public void When_18_or_older_THEN_no_validation_errors(
                    string dateOfBirth,
                    [Frozen] IDateTimeService dateTimeService,
                    RegistrationRequestValidator sut)
                {
                    dateTimeService.Now().Invoke().Returns(new DateTime(2018, 01, 01));

                    var registrationRequest = new RegistrationRequest
                    {
                        DateOfBirth = DateTime.Parse(dateOfBirth)
                    };

                    sut.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth, registrationRequest);
                }
            }

            public class EmailAddressTests
            {
                [Theory]
                [InlineAutoNSubstituteData("ab12@a1.co.uk")]
                [InlineAutoNSubstituteData("ab12@a1.com")]
                [InlineAutoNSubstituteData("abcdef@1234.co.uk")]
                [InlineAutoNSubstituteData("abcdef@1234.com")]
                [InlineAutoNSubstituteData("abcdef@abcd.co.uk")]
                [InlineAutoNSubstituteData("abcdef@abcd.com")]
                public void When_email_address_is_in_correct_format_THEN_no_validation_errors(
                    string emailAddress,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        EmailAddress = emailAddress
                    };

                    sut.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, registrationRequest);
                }

                [Theory]
                [InlineAutoNSubstituteData("ab1@a.co.uk")]
                [InlineAutoNSubstituteData("ab12@a1.baz")]
                public void When_email_address_has_incorrect_format_THEN_has_validation_errors(
                    string emailAddress,
                    RegistrationRequestValidator sut)
                {
                    var registrationRequest = new RegistrationRequest
                    {
                        EmailAddress = emailAddress
                    };

                    sut.ShouldHaveValidationErrorFor(x => x.EmailAddress, registrationRequest);
                }
            }

            [Theory, AutoNSubstituteData]
            public void When_email_address_and_date_of_birth_not_given_THEN_contains_validation_error(
                RegistrationRequestValidator sut)
            {
                var registrationRequest = new RegistrationRequest
                {
                    DateOfBirth = null,
                    EmailAddress = string.Empty
                };

                sut.ShouldHaveValidationErrorFor(x => x.EmailAddress, registrationRequest)
                    .WithErrorMessage("Must supply DOB or email address");

                sut.ShouldHaveValidationErrorFor(x => x.DateOfBirth, registrationRequest)
                    .WithErrorMessage("Must supply DOB or email address");
            }

            [Theory, AutoNSubstituteData]
            public void When_email_address_is_given_AND_date_of_birth_is_not_given_THEN_no_validation_errors(
                RegistrationRequestValidator sut)
            {
                var registrationRequest = new RegistrationRequest
                {
                    DateOfBirth = null,
                    EmailAddress = "abcd@123.co.uk"
                };

                sut.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, registrationRequest);

                sut.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth, registrationRequest);
            }

            [Theory, AutoNSubstituteData]
            public void When_email_address_not_given_AND_dat_of_birth_is_given_THEN_no_validation_errors(
                [Frozen] IDateTimeService dateTimeService,
                RegistrationRequestValidator sut)
            {
                dateTimeService.Now().Invoke().Returns(new DateTime(2020, 01, 01));

                var registrationRequest = new RegistrationRequest
                {
                    DateOfBirth = new DateTime(2000, 01, 01),
                    EmailAddress = string.Empty
                };

                sut.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, registrationRequest);

                sut.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth, registrationRequest);
            }
        }
    }
}
