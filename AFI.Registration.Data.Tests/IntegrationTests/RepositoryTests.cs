using System;
using System.Threading.Tasks;
using AFI.Registration.Data.Context;
using AFI.Registration.Data.Customizations;
using AFI.Registration.Data.Entities;
using AFI.Test.Helpers;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AFI.Registration.Data.Tests.IntegrationTests
{
    public class RepositoryTests : BaseTestClass<RegistrationContext>
    {
        [Theory, AutoNSubstituteData]
        public async Task When_new_customer_registers_THEN_customer_record_is_created(
            DateTime dateOfBirth, 
            Fixture fixture)
        {
            var customer = fixture.Customize(new CustomerCustomization
            {
                DateOfBirth = () => dateOfBirth
            }).Create<Customer>();

            await using var context = GetContext();
            var repository = new Repository(context);

            await repository.AddAsync(customer);

            var result = await repository.SaveChangesAsync();

            result.Should().Be(1);

            var savedCustomer = await context.Customers.FirstAsync(x => x.Id == 1);

            savedCustomer.Should().BeEquivalentTo(customer);
        }
    }
}
