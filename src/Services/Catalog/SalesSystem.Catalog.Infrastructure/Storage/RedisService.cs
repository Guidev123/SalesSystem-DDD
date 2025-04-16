using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SalesSystem.Catalog.Application.Storage;

namespace SalesSystem.Catalog.Infrastructure.Storage
{
    internal class RedisService(IDistributedCache cache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key)
        {
            var objectString = await cache.GetStringAsync(key) ?? string.Empty;

            return JsonConvert.DeserializeObject<T>(objectString);
        }

        public async Task SetAsync<T>(string key, T data)
        {
            var memoryCacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200),
            };

            var objectString = JsonConvert.SerializeObject(data);

            await cache.SetStringAsync(key, objectString, memoryCacheEntryOptions);
        }
    }
}
