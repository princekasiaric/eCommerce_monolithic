using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models.ProductAggregate
{
    public class ProductInventory : AuditableModelBase, IAggregateRoot
    {
        public long ProductId { get; set; }
        public int InitialTotalStock { get; set; }
        public int NewTotalStock { get; set; }
        public int QuantityAdded { get; set; }
        public decimal InitialUnitPrice { get; set; }
        public decimal NewUnitPrice { get; set; }
        public decimal InitialTotalPrice { get; set; }
        public decimal NewTotalPrice { get; set; }

        private ProductInventory()
        {

        }

        private ProductInventory(
            long userId,
            int initialTotalStock,
            int newTotalStock,
            int quantityAdded,
            decimal initialUnitPrice,
            decimal newUnitPrice,
            decimal initialTotalPrice,
            decimal newTotalPrice)
        {
            InitialTotalStock = initialTotalStock;
            NewTotalStock = newTotalStock;
            QuantityAdded = quantityAdded;
            InitialUnitPrice = initialUnitPrice;
            NewUnitPrice = newUnitPrice;
            InitialTotalPrice = initialTotalPrice;
            NewTotalPrice = newTotalPrice;
            Audit(userId, true);
        }

        public static ProductInventory Create(
            long userId,
            int initialTotalStock,
            int newTotalStock,
            int quantityAdded,
            decimal initialUnitPrice,
            decimal newUnitPrice,
            decimal initialTotalPrice,
            decimal newTotalPrice)
        {
            Guard.Against.NegativeOrZero(userId, nameof(userId));
            Guard.Against.Negative(initialTotalStock, nameof(initialTotalStock));
            Guard.Against.NegativeOrZero(newTotalStock, nameof(newTotalStock));
            Guard.Against.NegativeOrZero(quantityAdded, nameof(quantityAdded));
            Guard.Against.Negative(initialUnitPrice, nameof(initialUnitPrice));
            Guard.Against.NegativeOrZero(newUnitPrice, nameof(newUnitPrice));
            Guard.Against.Negative(initialTotalPrice, nameof(initialTotalPrice));
            Guard.Against.NegativeOrZero(newTotalPrice, nameof(newTotalPrice));
            var productInventory = new ProductInventory(
                userId,
                initialTotalStock,
                newTotalStock,
                quantityAdded,
                initialUnitPrice,
                newUnitPrice,
                initialTotalPrice,
                newTotalPrice);
            productInventory.Audit(userId, false);
            return productInventory;
        }
    }
}