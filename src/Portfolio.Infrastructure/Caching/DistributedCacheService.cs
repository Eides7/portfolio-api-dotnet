using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DistributedCacheService> _logger;

        public DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct)
        {
            var bytes = await _cache.GetAsync(key, ct);
            if (bytes is null || bytes.Length == 0)
            {
                _logger.LogInformation("CACHE MISS [{CacheKey}]", key);
                return default;
            }

            _logger.LogInformation("CACHE HIT [{CacheKey}]", key);

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

            _logger.LogInformation(
                "CACHE SET [{CacheKey}] (TTL: {TtlSeconds}s)",
                key,
                ttl.TotalSeconds);
        }

        public async Task RemoveAsync(string key, CancellationToken ct)
        {
           await _cache.RemoveAsync(key, ct);

            _logger.LogInformation("CACHE INVALIDATED [{CacheKey}]", key);
        }
    }
}
