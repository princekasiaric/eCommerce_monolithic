using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Construmart.Core.DataContracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.Domain.Models;
using Ardalis.GuardClauses;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.Domain.Models.OrderAggregate;

namespace Construmart.Infrastructure.Data.EfCore.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private bool _disposedValue = false;
        private readonly RepositoryContext _context;
        private IDbContextTransaction _transaction;
        private readonly IMediator _mediator;

        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Brand> _brandRepo;
        private readonly IRepository<Tag> _tagRepo;
        private readonly IRepository<Discount> _discountRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<ProductImage> _productImageRepo;
        private readonly IRepository<ProductInventory> _productInventoryRepo;
        private readonly IRepository<Cart> _cartRepo;
        private readonly IRepository<DeliveryAddress> _deliveryAddress;
        private readonly IRepository<NigerianState> _nigerianState;
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<Transaction> _transactionRepo;

        public RepositoryManager(
            RepositoryContext context,
            IMediator mediator,
            IRepository<Customer> customerRepo,
            IRepository<Category> categoryRepo,
            IRepository<Brand> brandRepo,
            IRepository<Tag> tagRepo,
            IRepository<Discount> discountRepo,
            IRepository<Product> productRepo,
            IRepository<ProductImage> productImageRepo,
            IRepository<ProductInventory> productInventoryRepo,
            IRepository<Cart> cartRepo,
            IRepository<DeliveryAddress> deliveryAddress,
            IRepository<NigerianState> nigerianState,
            IRepository<Order> order,
            IRepository<Transaction> transactionRepo)
        {
            _context = Guard.Against.Null(context, nameof(context));
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
            _customerRepo = Guard.Against.Null(customerRepo, nameof(customerRepo));
            _categoryRepo = Guard.Against.Null(categoryRepo, nameof(categoryRepo));
            _brandRepo = Guard.Against.Null(brandRepo, nameof(brandRepo));
            _tagRepo = Guard.Against.Null(tagRepo, nameof(tagRepo));
            _discountRepo = Guard.Against.Null(discountRepo, nameof(discountRepo));
            _productRepo = Guard.Against.Null(productRepo, nameof(productRepo));
            _productImageRepo = Guard.Against.Null(productImageRepo, nameof(productImageRepo));
            _productInventoryRepo = Guard.Against.Null(productInventoryRepo, nameof(productInventoryRepo));
            _cartRepo = Guard.Against.Null(cartRepo, nameof(cartRepo));
            _deliveryAddress = Guard.Against.Null(deliveryAddress, nameof(deliveryAddress));
            _nigerianState = Guard.Against.Null(nigerianState, nameof(nigerianState));
            _orderRepo = Guard.Against.Null(order, nameof(order));
            _transactionRepo = Guard.Against.Null(transactionRepo, nameof(transactionRepo));
        }

        public IRepository<Customer> CustomerRepo => _customerRepo;

        public IRepository<Category> CategoryRepo => _categoryRepo;

        public IRepository<Brand> BrandRepo => _brandRepo;

        public IRepository<Discount> DiscountRepo => _discountRepo;

        public IRepository<Tag> TagRepo => _tagRepo;

        public IRepository<Product> ProductRepo => _productRepo;

        public IRepository<ProductImage> ProductImageRepo => _productImageRepo;

        public IRepository<ProductInventory> ProductInventoryRepo => _productInventoryRepo;

        public IRepository<Cart> CartRepo => _cartRepo;

        public IRepository<DeliveryAddress> DeliveryAddressRepo => _deliveryAddress;

        public IRepository<NigerianState> NigerianStateRepo => _nigerianState;

        public IRepository<Order> OrderRepo => _orderRepo;

        public IRepository<Transaction> TransactionRepo => _transactionRepo;

        public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();

        public void BeginTransaction() => _transaction = _context.Database.BeginTransaction();

        public async Task SaveAsync()
        {
            await PublishEventsAsync();
            await _context.SaveChangesAsync();
        }

        public void Save()
        {
            PublishEventsAsync().Wait();
            _context.SaveChanges();
        }
        public async Task CommitAsync() => await _transaction.CommitAsync();
        public void Commit() => _transaction.Commit();

        public async Task RollbackAsync() => await _transaction.RollbackAsync();
        public void Rollback() => _transaction.Rollback();

        private async Task PublishEventsAsync()
        {
            var modelsWithEvent = _context.ChangeTracker.Entries<ModelBase>()
                ?.Select(po => po.Entity)
                ?.Where(po => po.DomainEvents != null && po.DomainEvents.Any())
                ?.ToArray();
            foreach (var model in modelsWithEvent)
            {
                var events = model.DomainEvents.ToArray();
                foreach (var domainEvent in events)
                {
                    await _mediator.Publish(domainEvent);
                    model.RemoveDomainEvent(domainEvent);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing && _context != null)
                {
                    //dispose managed state (managed objects)
                    _context.Dispose();
                }

                //free unmanaged resources (unmanaged objects) and override finalizer
                //set large fields to null
                _disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~RepositoryManager()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}