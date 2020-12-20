using System;
using System.Text.RegularExpressions;
using AFI.Registration.Services.Interfaces;
using AFI.Registration.Services.Models;
using FluentValidation;
using NodaTime;

namespace AFI.Registration.Services.Validation
{
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
    {
        private readonly IDateTimeService _dateTimeService;

        public RegistrationRequestValidator(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;

            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("First name too long")
                .MinimumLength(3).WithMessage("First name too short");

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Last name too long")
                .MinimumLength(3).WithMessage("Last name too short");

            RuleFor(x => x.ReferenceNumber).Matches(new Regex("^([A-Z]{2})(-)([0-9]{6})$"))
                .WithMessage("Reference is not in the correct format");

            RuleFor(x => x.EmailAddress).Matches(new Regex("^([A-Za-z0-9]{4,})(@)([A-Za-z0-9]{2,})(.co.uk|.com)$"))
                .WithMessage("email address is not in the correct format")
                .Unless(x=> string.IsNullOrWhiteSpace(x.EmailAddress));

            RuleFor(x => x.DateOfBirth).Must(dateOfBirth => BeOlderThanOrEqualTo(18, dateOfBirth))
                .WithMessage("Too young");

            When(x => x.DateOfBirth == null && string.IsNullOrWhiteSpace(x.EmailAddress), () =>
            {
                RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Must supply DOB or email address");
                RuleFor(x => x.DateOfBirth).NotNull().WithMessage("Must supply DOB or email address");
            });

        }

        private bool BeOlderThanOrEqualTo(int requiredAge, DateTime? dateOfBirth)
        {
            if (dateOfBirth == null)
            {
                return true;
            }

            var dateToday = _dateTimeService.Now().Invoke();

            var birthday = new LocalDate(dateOfBirth.Value.Year, dateOfBirth.Value.Month, dateOfBirth.Value.Day);

            var today = new LocalDate(dateToday.Year, dateToday.Month, dateToday.Day);

            var age = Period.Between(birthday, today, PeriodUnits.Years);

            return age.Years >= requiredAge;
        }
    }
}
