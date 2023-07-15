namespace Construmart.Core.Configurations
{
    public class CacheConfig
    {
        public double DefaultSlidingExpirationInSeconds { get; set; }
        public double DefaultAbsoluteExpirationInSeconds { get; set; }
        public string CacheSchema { get; set; }
        public string CacheTable { get; set; }
    }
}