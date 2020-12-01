using Blackdot.InvestigationSearch.Dtos;
using Blackdot.InvestigationSearch.Exceptions;
using Blackdot.InvestigationSearch.SearchEngines.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blackdot.InvestigationSearch.Queries.Handlers
{
    /// <summary>
    /// Query handler for GetSearchResultsQuery
    /// </summary>
    public class GetSearchResultsQueryHandler : IRequestHandler<GetSearchResultsQuery, SearchResult[]>
    {
        private IEnumerable<ISearchEngine> searchEngines;
        private readonly IValidator<string> searchTermValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchEngines">The search engines to run the query against</param>
        /// <param name="searchTermValidator">The validator for validating the search term</param>
        public GetSearchResultsQueryHandler(IEnumerable<ISearchEngine> searchEngines, IValidator<string> searchTermValidator)
        {
            this.searchEngines = searchEngines;
            this.searchTermValidator = searchTermValidator;
        }

        /// <summary>
        /// Handler for the GetSearchResultsQuery event
        /// </summary>
        /// <param name="request">The query event containing the search term</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>A collection of search results</returns>
        public async Task<SearchResult[]> Handle(GetSearchResultsQuery request, CancellationToken cancellationToken)
        {
            var validationResult = this.searchTermValidator.Validate(request.SearchTerm);

            if (!validationResult.IsValid) 
            {
                var errors = String.Join(Environment.NewLine, validationResult.Errors);
                throw new SearchTermValidationException(errors);
            }

            List<SearchResult> searchResults = new List<SearchResult>();

            var lockObject = new Object();

            var result = Parallel.ForEach(searchEngines, x =>
            {
                var results = x.GetSearchResultsAsync(request.SearchTerm);

                lock (lockObject)
                {
                    searchResults.AddRange(results);
                }
            });

            // distinct by url only
            searchResults = searchResults
                        .GroupBy(x => new { x.Url })
                        .Select(x => x.First())
                        .ToList();

            return searchResults.ToArray();
        }
    }
}
