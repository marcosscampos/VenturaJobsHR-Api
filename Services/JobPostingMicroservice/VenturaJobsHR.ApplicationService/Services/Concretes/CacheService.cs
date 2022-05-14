using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VenturaJobsHR.Domain.Aggregates.Common.Interfaces;
using VenturaJobsHR.Domain.Aggregates.Common.ValueObjects;

namespace VenturaJobsHR.Application.Services.Concretes;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<bool> ExistsAsync(string key, object value)
    {
        var cache = await _cache.GetStringAsync(key);
        if (string.IsNullOrWhiteSpace(cache))
            return false;

        var newValueJToken = JObject.Parse(JsonConvert.SerializeObject(value));
        var cacheJToken = JObject.Parse(cache);

        return JToken.DeepEquals(cacheJToken, newValueJToken);
    }

    public async Task<List<string>> FilterRequestsWithChangesAsync(IEnumerable<CacheRecord> items)
    {
        if (items?.Any() != true)
            return null;

        var itemsWithChanges = new List<string>();
        foreach (var item in items)
        {
            if (!await ExistsAsync($"{item.Key}", item.Value))
                itemsWithChanges.Add(item.Key);
        }

        return itemsWithChanges;
    }

    public async Task InsertOrUpdateAsync(string key, object value, DateTime expiration)
    {
        var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.Parse(expiration.ToString()) };

        await _cache.SetStringAsync($"{key}", JsonConvert.SerializeObject(value), options);
    }
}
