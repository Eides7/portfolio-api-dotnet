using Microsoft.Extensions.Caching.Distributed;
using Portfolio.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.Caching
{
    public sealed class DistributedCacheService : ICacheService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly IDistributedCache _cache;

        public DistributedCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct)
        {
            var bytes = await _cache.GetAsync(key, ct);
            if (bytes is null || bytes.Length == 0)
                return default;

            //deserialize the datas cached
            return JsonSerializer.Deserialize<T>(bytes, JsonOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct)
        {
            //Serialize the data before caching
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);

            await _cache.SetAsync(
                key,
                bytes,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl
                },
                ct);
        }

        public Task RemoveAsync(string key, CancellationToken ct)
            => _cache.RemoveAsync(key, ct);
    }
}
