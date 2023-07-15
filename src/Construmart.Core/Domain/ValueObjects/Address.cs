using System.Collections.Generic;
using Ardalis.GuardClauses;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.ValueObjects
{
    public class Address : ValueObjectBase
    {
        public string StreetNumber { get; private set; }
        public string StreetName { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }

        private Address(string streetNumber, string streetName, string state, string zipCode)
        {
            StreetNumber = streetNumber;
            StreetName = streetName;
            State = state;
            ZipCode = zipCode;
        }

        public static Address Create(string streetNumber, string streetName, string state, string zipCode)
        {
            Guard.Against.NullOrWhiteSpace(streetNumber, nameof(streetNumber));
            Guard.Against.NullOrWhiteSpace(streetNumber, nameof(streetNumber));
            Guard.Against.NullOrWhiteSpace(state, nameof(state));
            Guard.Against.NullOrWhiteSpace(zipCode, nameof(zipCode));
            return new Address(streetNumber, streetName, state, zipCode);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StreetNumber;
            yield return StreetName;
            yield return State;
            yield return ZipCode;
        }
    }
}