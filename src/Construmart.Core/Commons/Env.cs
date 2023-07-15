using System;

namespace Construmart.Core.Commons
{
    public class Env
    {
        private static string construmartDb = Environment.GetEnvironmentVariable("CONSTRUMART_DB");
        private static string jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        private static string jwtValidAudience = Environment.GetEnvironmentVariable("JWT_VALID_AUDIENCE");
        private static string jwtValidIssuer = Environment.GetEnvironmentVariable("JWT_VALID_ISSUER");
        private static string emailFromAddress = Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS");
        private static string emailFromName = Environment.GetEnvironmentVariable("EMAIL_FROM_NAME");
        private static string emailHost = Environment.GetEnvironmentVariable("EMAIL_HOST");
        private static string emailPort = Environment.GetEnvironmentVariable("EMAIL_PORT");
        private static string emailUserName = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
        private static string emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        private static string cloudinaryCloud = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD");
        private static string cloudinaryKey = Environment.GetEnvironmentVariable("CLOUDINARY_APIKEY");
        private static string cloudinarySecret = Environment.GetEnvironmentVariable("CLOUDINARY_SECRET");
        private static string payStackSecret = Environment.GetEnvironmentVariable("PAYSTACK_SECRET");

        public static string ConstrumartDb { get => construmartDb; set => construmartDb = value; }
        public static string JwtSecret { get => jwtSecret; set => jwtSecret = value; }
        public static string JwtValidAudience { get => jwtValidAudience; set => jwtValidAudience = value; }
        public static string JwtValidIssuer { get => jwtValidIssuer; set => jwtValidIssuer = value; }
        public static string EmailFromAddress { get => emailFromAddress; set => emailFromAddress = value; }
        public static string EmailFromName { get => emailFromName; set => emailFromName = value; }
        public static string EmailHost { get => emailHost; set => emailHost = value; }
        public static string EmailPort { get => emailPort; set => emailPort = value; }
        public static string EmailUserName { get => emailUserName; set => emailUserName = value; }
        public static string EmailPassword { get => emailPassword; set => emailPassword = value; }
        public static string CloudinaryCloud { get => cloudinaryCloud; set => cloudinaryCloud = value; }
        public static string CloudinaryKey { get => cloudinaryKey; set => cloudinaryKey = value; }
        public static string CloudinarySecret { get => cloudinarySecret; set => cloudinarySecret = value; }
        public static string PayStackSecret { get => payStackSecret; set => payStackSecret = value; }
    }
}