namespace Construmart.Core.Configurations
{
    public class AuthConfig
    {
        public string JwtSecret { get; set; }
        public string JwtValidIssuer { get; set; }
        public string JwtValidAudience { get; set; }
    }
}