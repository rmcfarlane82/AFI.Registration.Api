using System;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace AFI.Registration.Data.Entities
{
    public class Customer : EntityBase
    {
        // ReSharper disable once UnusedMember.Global Used by ef core
        protected Customer(){}

        public Customer(string firstName, string lastName, string referenceNumber, DateTime? dateOfBirth, string emailAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            ReferenceNumber = referenceNumber;
            DateOfBirth = dateOfBirth;
            EmailAddress = emailAddress;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string ReferenceNumber { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public string EmailAddress { get; private set; }
    }
}
