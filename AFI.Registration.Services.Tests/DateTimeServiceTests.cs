using System;
using AFI.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace AFI.Registration.Services.Tests
{
    public class DateTimeServiceTests
    {
        [Theory, AutoNSubstituteData]
        public void When_requesting_datetime_now_THEN_should_receive_datetime_now(
            DateTimeService sut)
        {
            var dateNow = sut.Now();

            dateNow().Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}
