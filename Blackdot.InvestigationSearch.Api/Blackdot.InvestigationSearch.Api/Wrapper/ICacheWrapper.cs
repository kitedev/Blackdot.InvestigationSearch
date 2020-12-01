using Blackdot.InvestigationSearch.Api.Dtos;
using System.Collections.Generic;

namespace Blackdot.InvestigationSearch.Api.Wrapper
{
    public interface ICacheWrapper
    {
        /// <summary>
        /// Set the cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="value">cache value</param>
        void Set(string key, List<SearchResponse> value);

        /// <summary>
        /// Get value from cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="result">cache result</param>
        /// <returns></returns>
        bool TryGetValue(string key, out List<SearchResponse> result);
    }
}
