using System.Collections.Generic;

namespace Construmart.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Routes
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ROOT = "api/v1/[controller]";
        /// <summary>
        /// 
        /// </summary>
        #region Auth Controller Routes
        public const string LOGIN = "login";
        /// <summary>
        /// 
        /// </summary>
        public const string CHANGE_PASSWORD = "password/change";
        #endregion

        #region Customer Controller Routes
        /// <summary>
        /// 
        /// </summary>
        public const string INITIATE_CUSTOMER_SIGNUP = "";
        /// <summary>
        /// 
        /// </summary>
        public const string COMPLETE_CUSTOMER_SIGNUP = "";
        /// <summary>
        /// 
        /// </summary>
        public const string INITIATE_RESET_PASSWORD = "password/reset";
        /// <summary>
        /// 
        /// </summary>
        public const string COMPLETE_RESET_PASSWORD = "password/reset";
        /// <summary>
        /// 
        /// </summary>
        public const string CUSTOMER_VIEW_ACCOUNT_DETAILS = "profile";
        /// <summary>
        /// 
        /// </summary>
        public const string CUSTOMER_UPDATE_ACCOUNT_DETAILS = "profile";
        /// <summary>
        /// 
        /// </summary>
        public const string VERIFY_OTP = "otp/verify";
        /// <summary>
        /// 
        /// </summary>
        public const string SEND_OTP = "otp/send";
        #endregion


        #region Categories Controller Routes
        public const string CREATE_CATEGORY = "";
        public const string GET_CATEGORIES = "";
        public const string GET_CATEGORY = "{id}";
        public const string UPDATE_CATEGORY = "{id}";
        public const string GET_PRODUCTS_BY_CATEGORY_ID = "{id}/products";
        #endregion

        #region Brands Controller Routes
        public const string CREATE_BRAND = "";
        public const string GET_BRANDS = "";
        public const string GET_BRAND = "{id}";
        public const string UPDATE_BRAND = "{id}";
        #endregion

        #region Tags Controller Routes
        public const string CREATE_TAG = "";
        public const string GET_TAGS = "";
        public const string GET_TAG = "{id}";
        public const string UPDATE_TAG = "{id}";
        public const string GET_PRODUCTS_BY_TAG_ID = "{id}/products";
        #endregion

        #region Discounts Controller Routes
        public const string CREATE_DISCOUNT = "";
        public const string GET_DISCOUNTS = "";
        public const string GET_DISCOUNT = "{id}";
        public const string UPDATE_DISCOUNT = "{id}";
        #endregion

        #region Products Controller Routes
        public const string CREATE_PRODUCT = "";
        public const string GET_PRODUCTS = "";
        public const string GET_PRODUCT = "{id}";
        public const string UPDATE_PRODUCT = "{id}";
        public const string PRODUCT_IMAGE_UPLOAD = "{id}/images";
        #endregion

        #region Carts Controller Routes
        public const string CREATE_CART = "";
        public const string GET_CART = "{id}";
        public const string UPDATE_CART = "{id}";
        public const string CLEAR_CART = "{id}";
        public const string GET_CARTITEM = "{cartId}/product/{productId}";
        public const string CLEAR_CARTITEM = "{cartId}/cartItem/{cartItemId}";
        #endregion

        #region DeliveryAddress Controller Routes
        public const string CREATE_DELIVERY_ADDRESS = "";
        public const string GET_DELIVERY_ADDRESSES = "";
        public const string GET_DELIVERY_ADDRESS = "{id}";
        public const string UPDATE_DELIVERY_ADDRESS = "{id}";
        public const string DELETE_DELIVERY_ADDRESS = "{id}";
        #endregion

        #region NigerianStates Controller Routes
        public const string GET_NIGERIAN_STATE = "{id}";
        public const string GET_NIGERIAN_STATES = "";
        #endregion

        #region Orde Controller Routes
        public const string CREATE_ORDER = "";
        public const string GET_ORDERS = "";
        public const string GET_ORDER = "{id:int}";
        public const string GET_ORDER_BY_TRACKING_NUMBER = "{trackingNumber}";
        public const string DELETE_ORDER = "{id}";
        #endregion
    }
}