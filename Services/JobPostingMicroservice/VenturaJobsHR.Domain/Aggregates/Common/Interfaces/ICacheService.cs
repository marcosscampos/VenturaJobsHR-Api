using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VenturaJobsHR.Domain.Aggregates.Common.ValueObjects;

namespace VenturaJobsHR.Domain.Aggregates.Common.Interfaces;

public interface ICacheService
{
    Task<List<string>> FilterRequestsWithChangesAsync(IEnumerable<CacheRecord> items);
    Task InsertOrUpdateAsync(string key, object value, DateTime expiration);
    Task<bool> ExistsAsync(string key, object value);
}
