using System;
using System.Collections.Generic;
using Construmart.Core.Commons.Utils;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.DTOs.Request
{
    //auth requests
    public record ChangePasswordRequest(string OldPassword, string NewPassword, string ConfirmPassword);
    public record LoginRequest(string Email, string Password);
    public record InitiateResetPaswordRequest(string Email);
    public record CompleteResetPasswordRequest(string Email, string Otp, string Password, string ConfirmPassword);

    //customer requests
    public record CompleteCustomerSignupRequest(string Email, string Otp);
    public record InitiateCustomerSignupRequest(string FirstName, string LastName, string Email, int Gender, string Password);
    public record UpdateCustomerDetailsRequest(string PhoneNumber, string StreetNumber, string StreetName, string State, string ZipCode);

    //category requests
    public record CategoryRequest(string Name, bool IsActive, bool IsParent, long? ParentCategoryId);
    public record FilterCategoriesParam(bool? IsActive, bool? IsParent, string SearchTerm);

    //brand requests
    public record BrandRequest(string Name);

    //tag requests
    public record TagRequest(string Name);

    //discount requests
    public record DiscountRequest(string Name, double PercentageOff);

    //product requests
    public record ProductRequest(
        long BrandId,
        long? DiscountId,
        IList<long> CategoryIds,
        IList<long> TagIds,
        string Name,
        string Description,
        int Quantity,
        decimal UnitPrice,
        bool IsActive);
    public class FilterProductsParam : QueryParameters
    {
        public bool? IsActive { get; set; }
        public string SearchTerm { get; set; }
    }

    public class CategoryParam : QueryParameters
    {
    }

    public class TagParam : QueryParameters
    {
    }
    //cart requests
    public record CartRequest(long ProductId, int Quantity);

    //miscellaneous
    public record FileRequest(IFormFile ImageFile);

    public record ImageUploadRequest(string Base64String);

    // delivery address request
    public record DeliveryAddressRequest(string Address, string City, string LGA, int StateID);

    public record OrderRequest(long CartId, string FirstName, string LastName, string PhoneNumber, long DeliveryAddressId, string PaymentRefrence);

    public class FilterOrdersParam : QueryParameters
    {
        public string TrackingNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SearchTerm { get; set; }
    }
}