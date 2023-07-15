using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models
{
    public class DeliveryAddress : AuditableModelBase, IAggregateRoot
    {
        public long CustomerId { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string LGA { get; private set; }
        public int NigerianStateId { get; private set; }

        private DeliveryAddress()
        {
        }

        private DeliveryAddress(
            long customerId,
            string address,
            string city,
            string lga,
            int stateId)
        {
            CustomerId = customerId;
            Address = address;
            City = city;
            LGA = lga;
            NigerianStateId = stateId;
        }

        public static DeliveryAddress Create(
            long customerId,
            string address,
            string city,
            string lga,
            int stateId)
        {
            Guard.Against.NegativeOrZero(customerId, nameof(customerId));
            Guard.Against.NullOrWhiteSpace(address, nameof(address));
            Guard.Against.NullOrWhiteSpace(city, nameof(city));
            Guard.Against.NullOrWhiteSpace(lga, nameof(lga));
            Guard.Against.NegativeOrZero(stateId, nameof(stateId));
            return new DeliveryAddress(customerId, address, city, lga, stateId);
        }

        public void Update(
            string address,
            string city,
            string lga,
            int stateId)
        {
            Address = Guard.Against.NullOrWhiteSpace(address, nameof(address));
            City = Guard.Against.NullOrWhiteSpace(city, nameof(city));
            LGA = Guard.Against.NullOrWhiteSpace(lga, nameof(lga));
            NigerianStateId = Guard.Against.NegativeOrZero(stateId, nameof(stateId));
        }
    }
}
