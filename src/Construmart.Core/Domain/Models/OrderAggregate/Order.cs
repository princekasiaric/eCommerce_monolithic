using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models.OrderAggregate
{
    public class Order : AuditableModelBase, IAggregateRoot
    {
        private readonly List<OrderItem> _orderItems;
        public IList<OrderItem> OrderItems => _orderItems;

        private Order()
        {
            _orderItems = new();
        }

        private Order(
            long customerId,
            string trackingNumber,
            string firstName,
            string lastName,
            string phoneNumber,
            string address,
            string city,
            string localGovernmentArea,
            string state,
            OrderStatus orderStatus) : this()
        {
            CustomerId = customerId;
            TrackingNumber = trackingNumber;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
            City = city;
            LocalGovernmentArea = localGovernmentArea;
            State = state;
            OrderStatus = orderStatus;
        }

        public long CustomerId { get; private set; }
        public long? DriverId { get; private set; }
        public string DriverFirstName { get; private set; }
        public string DriverLastName { get; private set; }
        public string TrackingNumber { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string LocalGovernmentArea { get; private set; }
        public string State { get; private set; }
        public OrderStatus OrderStatus { get; set; }

        public static Order Create(
            long customerId,
            string trackingNumber,
            string firstName,
            string lastName,
            string phoneNumber,
            string address,
            string city,
            string localGovernmentArea,
            string state,
            OrderStatus orderStatus
        )
        {
            Guard.Against.NegativeOrZero(customerId, nameof(customerId));
            Guard.Against.NullOrWhiteSpace(trackingNumber, nameof(trackingNumber));
            Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
            Guard.Against.NullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            Guard.Against.NullOrWhiteSpace(address, nameof(address));
            Guard.Against.NullOrWhiteSpace(localGovernmentArea, nameof(localGovernmentArea));
            Guard.Against.NullOrWhiteSpace(state, nameof(state));
            Guard.Against.Null(orderStatus, nameof(orderStatus));
            Order order = new(
                customerId,
                trackingNumber,
                firstName,
                lastName,
                phoneNumber,
                address,
                city,
                localGovernmentArea,
                state,
                orderStatus
            );
            return order;
        }

        public void AddOrderItem(long productId, string productName, decimal unitPrice, int quantity, double discount)
        {
            var orderItem = OrderItem.Create(
               productId,
               productName,
               unitPrice,
               quantity,
               discount
            );
            _orderItems.Add(orderItem);
        }

        public void SetOrderTotalAmount(decimal totalAmount)
        {
            TotalAmount = Guard.Against.NegativeOrZero(totalAmount, nameof(totalAmount));
        }
    }
}