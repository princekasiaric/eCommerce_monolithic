using System;

namespace Construmart.Core.Commons
{
    public enum ResponseCodes
    {
        [ResponseCodeDescriber("ERR001", "Our services are unreachable at this time. Please try again later.")]
        GeneralError = 1,

        [ResponseCodeDescriber("ERR002", "Request validation failed.")]
        RequestValidationFailure,

        [ResponseCodeDescriber("ERR003", "Invalid client.")]
        InvalidClient,

        [ResponseCodeDescriber("ERR004", "Invalid login credentials.")]
        InvalidLoginCredentials,

        [ResponseCodeDescriber("ERR005", "User account is locked or inactive.")]
        UserLockedOrInactive,

        [ResponseCodeDescriber("ERR006", "User signup failed.")]
        UserSignupFailure,

        [ResponseCodeDescriber("ERR007", "Email has been taken. Please try again.")]
        EmailTaken,

        [ResponseCodeDescriber("ERR008", "Otp verification failed. Please try again.")]
        InvalidOtp,

        [ResponseCodeDescriber("ERR009", "User account does not exist on this platform. Please signup or contact administrator.")]
        InvalidUserAccount,

        [ResponseCodeDescriber("ERR010", "Failed to activate user account. Please contact administrator.")]
        UserActivationFailure,

        [ResponseCodeDescriber("ERR011", "Failed to to change password.")]
        ChangePasswordFailure,

        [ResponseCodeDescriber("ERR012", "Category name exists on the platform.")]
        DuplicateCategory,

        [ResponseCodeDescriber("ERR013", "Parent category does not exist on the platform.")]
        InvalidParentCategory,

        [ResponseCodeDescriber("ERR014", "Category does not exist on the platform.")]
        InvalidCategory,

        [ResponseCodeDescriber("ERR015", "Brand name exists on the platform.")]
        DuplicateBrand,

        [ResponseCodeDescriber("ERR016", "Brand does not exist on the platform.")]
        InvalidBrand,

        [ResponseCodeDescriber("ERR017", "Tag name exists on the platform.")]
        DuplicateTag,

        [ResponseCodeDescriber("ERR018", "Tag does not exist on the platform.")]
        InvalidTag,

        [ResponseCodeDescriber("ERR019", "Discount name exists on the platform.")]
        DuplicateDiscount,

        [ResponseCodeDescriber("ERR020", "Discount does not exist on the platform.")]
        InvalidDiscount,

        [ResponseCodeDescriber("ERR021", "Product name exists on the platform.")]
        DuplicateProduct,

        [ResponseCodeDescriber("ERR022", "Product does not exist on the platform.")]
        InvalidProduct,

        [ResponseCodeDescriber("ERR023", "No record found.")]
        RecordNotFound,

        [ResponseCodeDescriber("ERR024", "File upload failed. Please try again.")]
        FileUploadFailure,

        [ResponseCodeDescriber("ERR025", "Cart does not exist on the platform.")]
        InvalidCart,

        [ResponseCodeDescriber("ERR026", "Cart is empty.")]
        EmptyCart,

        [ResponseCodeDescriber("ERR027", "Cart Item does not exist on the platform.")]
        InvalidCartItem,

        [ResponseCodeDescriber("ERR028", "Delivery address does not exist on the platform.")]
        InvalidDeliveryAddress,

        [ResponseCodeDescriber("ERR029", "State does not exist on the platform.")]
        InvalidNigerianState,

        [ResponseCodeDescriber("ERR030", "LGA does not exist.")]
        InvalidLGA,

        [ResponseCodeDescriber("ERR031", "Payment processor failed to verify your transaction.")]
        TransactionVerification,

        [ResponseCodeDescriber("ERR032", "Transaction does not exist on payment processor plateform.")]
        InvalidTransaction
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    sealed class ResponseCodeDescriberAttribute : Attribute
    {
        public ResponseCodeDescriberAttribute(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; }
        public string Description { get; }
    }

    public static class ResponseCodeExtension
    {
        public static string GetCode(this ResponseCodes responseCode)
        {
            var type = typeof(ResponseCodes);
            var property = type.GetField(responseCode.ToString());
            var attribute = (ResponseCodeDescriberAttribute[])property.GetCustomAttributes(typeof(ResponseCodeDescriberAttribute), false);
            return attribute[0].Code;
        }

        public static string GetDescription(this ResponseCodes responseCode)
        {
            var type = typeof(ResponseCodes);
            var property = type.GetField(responseCode.ToString());
            var attribute = (ResponseCodeDescriberAttribute[])property.GetCustomAttributes(typeof(ResponseCodeDescriberAttribute), false);
            return attribute[0].Description;
        }
    }
}