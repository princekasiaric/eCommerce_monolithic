using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;
using Construmart.Core.Configurations;
using Construmart.Core.ProcessorContracts.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Construmart.Infrastructure.Processors.Cache
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly CacheConfig _cacheConfig;
        private TimeSpan _slidingExpirationTime;
        private TimeSpan _absoluteExpirationTime;

        public DistributedCacheService(IDistributedCache cache, IOptions<CacheConfig> cacheConfigOptions)
        {
            _cache = cache;
            _cacheConfig = cacheConfigOptions.Value;
        }

        private void ConfigureCache(double slidingExpiratonInSeconds, double absoluteExpirationInSeconds)
        {
            _slidingExpirationTime = TimeSpan.FromSeconds(slidingExpiratonInSeconds);
            _absoluteExpirationTime = TimeSpan.FromSeconds(absoluteExpirationInSeconds);
        }

        public async Task<T> FetchDataAsync<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var data = await _cache.GetAsync(key).ConfigureAwait(false);
            if (data == null) return default;
            using var ms = new MemoryStream(data);
            var obj = await JsonSerializer.DeserializeAsync<T>(ms);
            return obj;
        }

        public async Task SaveDataAsync<T>(string key, T value, double slidingExpiratonInSeconds = 0, double absoluteExpirationInSeconds = 0)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            using var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, (Object)value);
            var byteValue = ms.ToArray();
            slidingExpiratonInSeconds = slidingExpiratonInSeconds <= 0 ? _cacheConfig.DefaultSlidingExpirationInSeconds : slidingExpiratonInSeconds;
            absoluteExpirationInSeconds = absoluteExpirationInSeconds <= 0 ? _cacheConfig.DefaultAbsoluteExpirationInSeconds : absoluteExpirationInSeconds;
            ConfigureCache(slidingExpiratonInSeconds, absoluteExpirationInSeconds);
            if (slidingExpiratonInSeconds == 0 || absoluteExpirationInSeconds == 0)
            {
                await _cache.SetAsync(key, byteValue).ConfigureAwait(false);
            }
            else
            {
                await _cache.SetAsync(
                    key,
                    byteValue,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now + _absoluteExpirationTime,
                        AbsoluteExpirationRelativeToNow = _absoluteExpirationTime,
                        SlidingExpiration = _slidingExpirationTime
                    }
                ).ConfigureAwait(false);
            }
        }

        public async Task RemoveDataAsync(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            await _cache.RemoveAsync(key).ConfigureAwait(false);
        }
    }
}