using System;
using System.Linq;
using System.Threading.Tasks;
using AFI.Registration.Api.ExceptionHanding;
using AFI.Registration.Api.Models;
using AFI.Registration.Services.Interfaces;
using AFI.Registration.Services.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AFI.Registration.Api.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerRegistrationController : ControllerBase
    {
        private readonly ILogger<CustomerRegistrationController> _logger;
        private readonly ICustomerRegistrationService _customerRegistrationService;

        public CustomerRegistrationController(
            ILogger<CustomerRegistrationController> logger, 
            ICustomerRegistrationService customerRegistrationService)
        {
            _logger = logger;
            _customerRegistrationService = customerRegistrationService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            try
            {
                var newCustomer = await _customerRegistrationService.Register(registrationRequest);

                return Created("api/customer", new CustomerRegistrationResult(newCustomer.Id));
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return Problem("Sorry an error occurred");
            }
        }
    }
}
