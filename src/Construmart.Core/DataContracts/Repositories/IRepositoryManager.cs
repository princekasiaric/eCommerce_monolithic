using System;
using System.Threading.Tasks;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.OrderAggregate;
using Construmart.Core.Domain.Models.ProductAggregate;

namespace Construmart.Core.DataContracts.Repositories
{
    public interface IRepositoryManager : IDisposable
    {
        IRepository<Customer> CustomerRepo { get; }
        IRepository<Category> CategoryRepo { get; }
        IRepository<Brand> BrandRepo { get; }
        IRepository<Tag> TagRepo { get; }
        IRepository<Discount> DiscountRepo { get; }
        IRepository<Product> ProductRepo { get; }
        IRepository<ProductImage> ProductImageRepo { get; }
        IRepository<ProductInventory> ProductInventoryRepo { get; }
        IRepository<Cart> CartRepo { get; }
        IRepository<DeliveryAddress> DeliveryAddressRepo { get; }
        IRepository<NigerianState> NigerianStateRepo { get; }
        IRepository<Order> OrderRepo { get; }
        IRepository<Transaction> TransactionRepo { get; }
        Task BeginTransactionAsync();
        void BeginTransaction();
        Task SaveAsync();
        void Save();
        Task CommitAsync();
        void Commit();
        Task RollbackAsync();
        void Rollback();
    }
}