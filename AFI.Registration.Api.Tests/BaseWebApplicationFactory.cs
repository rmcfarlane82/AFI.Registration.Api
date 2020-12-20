using Microsoft.AspNetCore.Mvc.Testing;

namespace AFI.Registration.Api.Tests
{
    public class BaseWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup: class
    {
    }
}
