using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models.OrderAggregate
{
    public class OrderItem : AuditableModelBase, IAggregateRoot
    {
        private OrderItem()
        {

        }

        private OrderItem(
            long productId,
            string productName,
            decimal unitPrice,
            int quantity,
            double discount)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
        }

        public long OrderId { get; private set; }
        public long ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public double Discount { get; private set; }

        public static OrderItem Create(
            long productId,
            string productName,
            decimal unitPrice,
            int quantity,
            double discount)
        {
            Guard.Against.NegativeOrZero(productId, nameof(productId));
            Guard.Against.NullOrWhiteSpace(productName, nameof(productName));
            Guard.Against.NegativeOrZero(unitPrice, nameof(unitPrice));
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            Guard.Against.Negative(discount, nameof(discount));
            var orderItem = new OrderItem(productId, productName, unitPrice, quantity, discount);
            return orderItem;
        }
    }
}