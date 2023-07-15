using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models
{
    public class Cart : AuditableModelBase, IAggregateRoot
    {
        private readonly List<CartItem> _cartItems;
        public bool HasCheckout { get; private set; }
        public IList<CartItem> CartItems => _cartItems;

        private Cart()
        {
            _cartItems = new();
        }

        public static Cart Create(long productId, int quantity)
        {
            Guard.Against.NegativeOrZero(productId, nameof(productId));
            Guard.Against.NegativeOrZero(quantity, nameof(quantity));

            Cart cart = new();

            var cartItem = CartItem.Create(productId, quantity);

            cart._cartItems.Add(cartItem);

            return cart;
        }

        public void Update(long productId, int quantity)
        {
            var cartItem = _cartItems.SingleOrDefault(x => x.ProductId == productId);
            if (cartItem == null)
            {
                _cartItems.Add(CartItem.Create(productId, quantity));
            }
            else
            {
                cartItem.UpdateItem(quantity);
            }
        }

        public bool RemoveCartItem(CartItem cartItem)
        {
            var isRemoved = _cartItems.Remove(cartItem);
            return isRemoved;
        }

        public void Checkout() => HasCheckout = true;
    }
}
