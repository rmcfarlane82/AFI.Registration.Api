using System;
using AFI.Registration.Data.Entities;
using AutoFixture;

namespace AFI.Registration.Data.Customizations
{
    public class CustomerCustomization : ICustomization
    {
        public Func<string> FirstName { get; set; }
        public Func<string> LastName { get; set; }
        public Func<string> ReferenceNumber { get; set; }
        public Func<string> EmailAddress { get; set; }
        public Func<DateTime?> DateOfBirth { get; set; }

        public void Customize(IFixture fixture)
        {
            fixture.Behaviors.Clear();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Customize<Customer>(x=> x.FromFactory(() => new Customer(
                FirstName?.Invoke() ?? fixture.Create<string>(),
                LastName?.Invoke() ?? fixture.Create<string>(),
                ReferenceNumber?.Invoke() ?? fixture.Create<string>().Substring(0,9),
                DateOfBirth?.Invoke(),
                EmailAddress?.Invoke() ?? fixture.Create<string>()
            )));
        }
    }
}
