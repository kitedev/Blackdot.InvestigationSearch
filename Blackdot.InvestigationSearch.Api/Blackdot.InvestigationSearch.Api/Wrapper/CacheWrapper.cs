using Blackdot.InvestigationSearch.Api.Dtos;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace Blackdot.InvestigationSearch.Api.Wrapper
{
    /// <summary>
    /// A wrapper for server side caching
    /// </summary>
    public class CacheWrapper: ICacheWrapper
    { 
        private IMemoryCache memoryCache;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="memoryCache">The instance of Microsoft.Extensions.Caching.Memory.MemoryCache</param>
        public CacheWrapper(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Set the cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="value">cache value</param>
        public void Set(string key, List<SearchResponse> value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()

            //TODO: Read expiration times from config

            // Keep in cache for this time, reset time if accessed.
            .SetSlidingExpiration(TimeSpan.FromSeconds(600)) // 10 minutes
                                                                // set absolute expiration, to stop cache becoming stale
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(1200)); // 20 minutes
            this.memoryCache.Set(key, value, cacheEntryOptions);
        }

        /// <summary>
        /// Get value from cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="result">cache result</param>
        public bool TryGetValue(string key, out List<SearchResponse> result)
        {
            var isValueExist = this.memoryCache.TryGetValue(key, out List<SearchResponse> cacheResult);
            result = cacheResult;
            return isValueExist;
        }
    }
}
