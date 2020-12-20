using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AFI.Registration.Api.ExceptionHanding;
using AFI.Registration.Api.Models;
using AFI.Registration.Data.Entities;
using AFI.Registration.Services.Interfaces;
using AFI.Registration.Services.Models;
using AFI.Test.Helpers;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace AFI.Registration.Api.Tests
{
    public class CustomerControllerTests : IClassFixture<BaseWebApplicationFactory<Startup>>
    {
        private readonly BaseWebApplicationFactory<Startup> _factory;
        private const string CustomerEndpoint = "api/customer";

        public CustomerControllerTests(BaseWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory, AutoNSubstituteData]
        public async Task When_sending_a_invalid_registration_request_THEN_bad_request_is_returned_with_validation_errors(
            IFixture fixture
            )
        {
            var registrationRequest = fixture.Create<RegistrationRequest>();

            var client = _factory.CreateDefaultClient();

            var json = JsonConvert.SerializeObject(registrationRequest);

            var response = await client.PostAsync($"{CustomerEndpoint}/register", new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json));

            response.IsSuccessStatusCode.Should().BeFalse();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = JsonConvert.DeserializeObject<List<ValidationError>>(await response.Content.ReadAsStringAsync());

            content.Should().HaveCount(3)
                .And.BeEquivalentTo(new List<ValidationError>
                {
                    new ValidationError("ReferenceNumber", "Reference is not in the correct format"),
                    new ValidationError("EmailAddress", "email address is not in the correct format"),
                    new ValidationError("DateOfBirth", "Too young"),
                });
        }

        [Theory, AutoNSubstituteData]
        public async Task When_sending_a_valid_registration_request_THEN_customer_id_is_returned(
            IFixture fixture
            )
        {
            var registrationRequest = new RegistrationRequest
            {
                EmailAddress = "abcd@123.com",
                DateOfBirth = DateTime.Now.AddYears(-18),
                ReferenceNumber = "XX-123456",
                FirstName = fixture.Create<string>(),
                LastName = fixture.Create<string>()
            };

            var client = _factory.CreateDefaultClient();

            var json = JsonConvert.SerializeObject(registrationRequest);

            var response = await client.PostAsync($"{CustomerEndpoint}/register", new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json));

            response.IsSuccessStatusCode.Should().BeTrue();

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = JsonConvert.DeserializeObject<CustomerRegistrationResult>(await response.Content.ReadAsStringAsync());

            content.CustomerId.Should().Be(1);
        }

        [Theory, AutoNSubstituteData]
        public async Task When_exception_is_thrown_THEN_internal_server_error_is_returned(
            RegistrationRequest registrationRequest
            )
        {

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services => { services.AddScoped<ICustomerRegistrationService, ExceptionCustomerRegistrationService>(); });
            }).CreateClient();


            var json = JsonConvert.SerializeObject(registrationRequest);

            var response = await client.PostAsync($"{CustomerEndpoint}/register", new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json));

            response.IsSuccessStatusCode.Should().BeFalse();

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var content = JsonConvert.DeserializeObject<ProblemDetails>(await response.Content.ReadAsStringAsync());

            content.Detail.Should().Be("Sorry an error occurred");

        }

        private class ExceptionCustomerRegistrationService : ICustomerRegistrationService
        {
            public Task<Customer> Register(RegistrationRequest registrationRequest)
            {
                throw new Exception("This class always throws exception");
            }
        }
    }
}
