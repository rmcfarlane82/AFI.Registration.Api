using System;
using AFI.Registration.Services.Interfaces;

namespace AFI.Registration.Services
{
    public class DateTimeService : IDateTimeService
    {
        public Func<DateTime> Now()
        {
            return () => DateTime.Now;
        }
    }
}
