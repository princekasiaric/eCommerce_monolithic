using System;
using System.Collections.Generic;
using Construmart.Core.Domain.ValueObjects;

namespace Construmart.Core.DTOs.Response
{
    //base response records
    public record BaseResponse(bool IsSuccess, ErrorResponse Error, int StatusCode);
    public record ErrorResponse(string HttpMethod, string RequestPath, string ErrorCode, string Description, IList<string> Reasons = null);
    public record ServiceResponse<T>(bool IsSuccess, int StatusCode, T Payload) : BaseResponse(IsSuccess, null, StatusCode);
    public record CreatedResponse(long Id);

    //response records
    public record LoginResponse(string AccessToken, DateTime ValidTo);
    public record CustomerAccountInfoResponse(string FirstName, string LastName, string Gender, string Email, string PhoneNumber, Address Address);
    public record CategoryResponse(
        long Id,
        string Name,
        bool IsParent,
        bool IsActive,
        long? ParentCategoryId,
        DateTime DateCreated,
        DateTime? DateUpdated,
        long? CreatedByUserId,
        long? UpdatedByUserId);
    public record BrandResponse(
        long Id,
        string Name,
        DateTime DateCreated,
        DateTime? DateUpdated,
        long? CreatedByUserId,
        long? UpdatedByUserId);

    public record TagResponse(
        long Id,
        string Name,
        DateTime DateCreated,
        DateTime? DateUpdated,
        long? CreatedByUserId,
        long? UpdatedByUserId);

    public record DiscountResponse(
        long Id,
        string Name,
        double PercentageOff,
        DateTime DateCreated,
        DateTime? DateUpdated,
        long? CreatedByUserId,
        long? UpdatedByUserId);

    public record ProductResponse
    {
        public long Id { get; init; }
        public long? BrandId { get; init; }
        public long? DiscountId { get; init; }
        public string Sku { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal UnitPrice { get; init; }
        public string CurrencyCode { get; set; }
        public bool IsActive { get; init; }
        public IList<ProductCategoryResponse> ProductCategories { get; set; }
        public IList<ProductTagResponse> ProductTags { get; set; }
        public DateTime DateCreated { get; init; }
        public DateTime? DateUpdated { get; init; }
        public long? CreatedByUserId { get; init; }
        public long? UpdatedByUserId { get; init; }
        public string ImageUrl { get; init; }
        public string SecureImageUrl { get; init; }
    }

    public record ProductCategoryResponse
    {
        public long Id { get; init; }
        public string Name { get; set; }
    }

    public record ProductTagResponse
    {
        public long Id { get; init; }
        public string Name { get; set; }
    }

    public record UploadProductImageResponse(long ProductId, string Url, string SecureUrl);

    public record CartResponse
    {
        public long Id { get; init; }
        public bool HasCheckout { get; init; }
        public IList<CartItemResponse> CartItems { get; init; }
        public decimal TotalPrice { get; init; }
        public DateTime DateCreated { get; init; }
        public DateTime? DateUpdated { get; init; }
        public long? CreatedByUserId { get; init; }
        public long? UpdatedByUserId { get; init; }
    }

    public record CartItemResponse
    {
        public long Id { get; init; }
        public long ProductId { get; init; }
        public string ProductName { get; init; }
        public int Quantity { get; init; }
        public decimal Price { get; init; }
    }

    public record DeliveryAddressResponse
    {
        public long Id { get; init; }
        public long CustomerId { get; init; }
        public string Address { get; init; }
        public string City { get; init; }
        public string LGA { get; init; }
        public string State { get; set; }

    }

    public record NigerianStateResponse(long Id, string State, List<string> LocalGovernmentAreas);

    public record OrderResponse
    {
        public long Id { get; init; }
        public long CustomerId { get; init; }
        public string DriverFirstName { get; init; }
        public string DriverLastName { get; init; }
        public string TrackingNumber { get; init; }
        public decimal TotalAmount { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public string Address { get; init; }
        public string City { get; init; }
        public string LocalGovernmentArea { get; init; }
        public string State { get; init; }
        public string OrderStatus { get; init; }
        public IList<OrderItemResponse> OrderItems { get; set; }

    }

    public record OrderItemResponse
    {
        public long Id { get; init; }
        public long ProductId { get; init; }
        public string ProductName { get; init; }
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
        public double? Discount { get; init; }
    }

    // public record AuditResponse
    // {
    //     public DateTime DateCreated { get; init; } = default!;
    //     public DateTime? DateUpdated { get; init; } = default!;
    //     public long? CreatedByUserId { get; init; } = default!;
    //     public long? UpdatedByUserId { get; init; } = default!;
    // }
}