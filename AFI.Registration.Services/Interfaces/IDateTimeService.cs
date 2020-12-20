using System;

namespace AFI.Registration.Services.Interfaces
{
    public interface IDateTimeService
    {
        Func<DateTime> Now();
    }
}
