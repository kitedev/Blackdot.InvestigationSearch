using Blackdot.InvestigationSearch.Dtos;
using System.Collections.Generic;

namespace Blackdot.InvestigationSearch.SearchEngines.Interfaces
{
    public interface ISearchEngine
    {
        /// <summary>
        /// Get search results for a given search term
        /// </summary>
        /// <param name="searchTerm">The search term to find search results for</param>
        /// <returns>A collection of search results</returns>
        IEnumerable<SearchResult> GetSearchResults(string searchTerm);
    }
}
