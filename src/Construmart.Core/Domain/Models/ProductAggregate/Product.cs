using System.Collections.Generic;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;
using Ardalis.GuardClauses;
using System.Linq;
using Construmart.Core.Domain.Enumerations;

namespace Construmart.Core.Domain.Models.ProductAggregate
{
    public class Product : AuditableModelBase, IAggregateRoot
    {
        private readonly List<long> _productCategoryIds;
        private readonly List<long> _productTagIds;
        private readonly List<ProductInventory> _productInventories;

        public long? BrandId { get; private set; }
        public long? DiscountId { get; private set; }
        public long? ProductImageId { get; private set; }
        public string Sku { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal UnitPrice { get; private set; }
        public CurrencyCodes CurrencyCode { get; private set; }
        public bool IsActive { get; private set; }
        public ProductImage ProductImage;
        public IList<long> ProductCategoryIds => _productCategoryIds.AsReadOnly();
        public IList<long> ProductTagIds => _productTagIds;
        public IList<ProductInventory> ProductInventories => _productInventories.AsReadOnly();

        private Product()
        {
            _productCategoryIds = new List<long>();
            _productTagIds = new();
            _productInventories = new List<ProductInventory>();
        }

        private Product(
            long? brandId,
            long? discountId,
            IList<long> categoryIds,
            IList<long> tagIds,
            long userId,
            string sku,
            string name,
            string description,
            decimal unitPrice,
            CurrencyCodes currencyCode,
            bool isActive) : this()
        {
            BrandId = brandId;
            DiscountId = discountId;
            Sku = sku;
            Name = name;
            Description = description;
            UnitPrice = unitPrice;
            CurrencyCode = currencyCode;
            IsActive = isActive;
            foreach (var id in categoryIds)
            {
                if (!ProductCategoryIds.Contains(id))
                {
                    _productCategoryIds.Add(id);
                }
            }
            foreach (var id in tagIds)
            {
                if (!ProductTagIds.Contains(id))
                {
                    _productTagIds.Add(id);
                }
            }
            Audit(userId, true);
        }

        public static Product Create(
            long? brandId,
            long? discountId,
            IList<long> categoryIds,
            IList<long> tagIds,
            long userId,
            string sku,
            string name,
            string description,
            decimal unitPrice,
            CurrencyCodes currencyCode,
            bool isActive)
        {
            Guard.Against.NegativeOrZero(userId, nameof(userId));
            Guard.Against.NullOrWhiteSpace(sku, nameof(sku));
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(description, nameof(description));
            Guard.Against.NegativeOrZero(unitPrice, nameof(unitPrice));
            Guard.Against.Null(currencyCode, nameof(currencyCode));
            return new Product(brandId, discountId, categoryIds, tagIds, userId, sku, name, description, unitPrice, currencyCode, isActive);
        }

        public void Update(
            long? brandId,
            long? discountId,
            IList<long> categoryIds,
            IList<long> tagIds,
            long userId,
            string name,
            string description,
            decimal unitPrice,
            CurrencyCodes currencyCodes,
            bool isActive = true)
        {
            BrandId = brandId;
            DiscountId = discountId;
            if (ProductCategoryIds.Any())
                _productCategoryIds.Clear();

            if (ProductTagIds.Any())
                _productTagIds.Clear();

            foreach (var id in categoryIds)
                _productCategoryIds.Add(id);
            foreach (var id in tagIds)
                _productTagIds.Add(id);

            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Description = Guard.Against.NullOrWhiteSpace(description, nameof(description));
            UnitPrice = Guard.Against.NegativeOrZero(unitPrice, nameof(unitPrice));
            CurrencyCode = currencyCodes;
            IsActive = isActive;
            Audit(userId, false);
        }

        public void UploadImage(string url, string secureUrl)
        {
            ProductImage = ProductImage.Create(url, secureUrl, true);
        }

        public void UpdateInventory(
            long userId,
            int quantityAdded,
            decimal unitPrice)
        {
            if (_productInventories.Any())
            {
                var lastInventory = _productInventories.Last();
                var newTotalStock = lastInventory.InitialTotalStock + quantityAdded;
                var newTotalPrice = unitPrice * newTotalStock;
                _productInventories.Add(ProductInventory.Create(
                Guard.Against.NegativeOrZero(userId, nameof(userId)),
                lastInventory.InitialTotalStock,
                newTotalStock,
                Guard.Against.Zero(quantityAdded, nameof(quantityAdded)),
                lastInventory.NewUnitPrice,
                Guard.Against.Negative(unitPrice, nameof(unitPrice)),
                lastInventory.NewTotalPrice,
                newTotalPrice));
                return;
            }
            _productInventories.Add(ProductInventory.Create(
                Guard.Against.NegativeOrZero(userId, nameof(userId)),
                0,
                quantityAdded,
                Guard.Against.Zero(quantityAdded, nameof(quantityAdded)),
                0,
                Guard.Against.Negative(unitPrice, nameof(unitPrice)),
                0,
                unitPrice));
        }
    }
}