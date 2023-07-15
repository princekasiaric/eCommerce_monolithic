using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models
{
    public class CartItem : AuditableModelBase, IAggregateRoot
    {
        public long CartId { get; private set; }
        public long ProductId { get; private set; }
        public int Quantity { get; private set; }

        private CartItem()
        {
        }

        private CartItem(long productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public static CartItem Create(long productId, int quantity)
        {
            Guard.Against.NegativeOrZero(productId, nameof(productId));
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));
            return new CartItem(productId, quantity);
        }

        public void UpdateItem(int quantity)
        {
            Quantity = quantity;
        }
    }
}
