namespace AFI.Registration.Api.Models
{
    public class CustomerRegistrationResult
    {
        public CustomerRegistrationResult(int customerId)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get; }
    }
}
