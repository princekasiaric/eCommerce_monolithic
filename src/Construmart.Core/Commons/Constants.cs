using System.Net.Mime;
using Microsoft.Net.Http.Headers;

namespace Construmart.Core.Commons
{
    public static class Constants
    {
        public const string HEALTH_CHECK_PATH = "/health";
        public static class Cors
        {
            public static class Policy
            {
                public const string ALLOW_CONSTRUMART_ORIGIN = "CONSTRUMART";
            }

            public static class Origin
            {
                public const string CONSTRUMART_FRONTEND = "http://localhost";
            }
        }


        public static class AppRegex
        {
            public const string PASSWORD = @"^.*(?=.{6,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
            public const string PHONE_NUMBER = @"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$";
            public const string ALPHANUMERIC_WITH_COMMA = @"^[A-Za-z0-9\-_\s\,]+$";
            public const string ALPHANUMERIC = @"^[A-Za-z0-9\-_\s]+$";
            // public const string ALPHABET = @"^[A-Za-z\-_\s]+$";
            public const string ALPHABET = @"^([^0-9]*)$";
            public const string DIGIT = @"^[0-9]+$";
            public const string EMAIL = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
        }

        public static class CacheKeys
        {
            public const string CUSTOMER_SIGNUP = nameof(CUSTOMER_SIGNUP);
        }

        public static class NotificationTemplates
        {
            public const string FOLDER = "NotificationTemplates";
            public static class SignupOtp
            {
                public const string SIGNUP_OTP_TEMPLATE = "signup_otp.html";
                public static class Placeholders
                {
                    public const string OTP = "{{otp}}";
                }
            }
        }

        public static class CustomHeaderNames
        {
            public const string XClient = nameof(XClient);
        }
    }
}