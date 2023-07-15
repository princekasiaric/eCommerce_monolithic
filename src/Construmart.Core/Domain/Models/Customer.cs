using System;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Events;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.Domain.ValueObjects;

namespace Construmart.Core.Domain.Models
{
    public class Customer : AuditableModelBase, IAggregateRoot
    {
        public long ApplicationUserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Gender Gender { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        //public Transaction Address { get; private set; }
        // public Otp Otp { get; private set; }
        public CustomerOnboardingStatus OnboardingStatus { get; private set; }

        private Customer()
        {
        }

        private Customer(long applicationUserId, string firstName, string lastName, string email, Gender gender)
        {
            ApplicationUserId = Guard.Against.NegativeOrZero(applicationUserId, nameof(applicationUserId));
            FirstName = Guard.Against.NullOrEmpty(firstName, nameof(firstName));
            LastName = Guard.Against.NullOrEmpty(lastName, nameof(lastName));
            Email = Guard.Against.NullOrEmpty(email, nameof(email));
            Gender = Guard.Against.Null(gender, nameof(gender));
            OnboardingStatus = CustomerOnboardingStatus.Initiated;
            Audit(null, true);
        }

        public static Customer Create(
            long applicationUserId,
            string firstName,
            string lastName,
            string email,
            Gender gender)
        {
            Guard.Against.NegativeOrZero(applicationUserId, nameof(applicationUserId));
            Guard.Against.NullOrEmpty(firstName, nameof(firstName));
            Guard.Against.NullOrEmpty(lastName, nameof(lastName));
            Guard.Against.NullOrEmpty(email, nameof(email));
            Guard.Against.Null(gender, nameof(gender));
            return new Customer(applicationUserId, firstName, lastName, email, gender);
        }

        public void Update(string firstName, string lastName, Gender gender)
        {
            FirstName = Guard.Against.NullOrEmpty(firstName, nameof(firstName));
            LastName = Guard.Against.NullOrEmpty(lastName, nameof(lastName));
            Gender = Guard.Against.Null(gender, nameof(gender));
            Audit(null, false);
        }

        public void Update(string phoneNumber, string streetName, string streetNumber, string state, string zipcode)
        {
            PhoneNumber = Guard.Against.NullOrEmpty(phoneNumber, nameof(phoneNumber));
            Address = Address.Create(streetNumber, streetName, state, zipcode);
            Audit(null, false);
        }

        public void CompleteSignup()
        {
            OnboardingStatus = CustomerOnboardingStatus.Completed;
        }
    }
}