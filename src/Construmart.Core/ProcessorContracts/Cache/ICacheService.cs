using System.Threading.Tasks;

namespace Construmart.Core.ProcessorContracts.Cache
{
    public interface ICacheService
    {
        Task SaveDataAsync<T>(string key, T value, double slidingExpiratonInSeconds = 0, double absoluteExpirationInSeconds = 0);
        Task<T> FetchDataAsync<T>(string key);
        Task RemoveDataAsync(string key);
    }
}