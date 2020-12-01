using Blackdot.InvestigationSearch.Dtos;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blackdot.InvestigationSearch.Queries.SearchEngineB.Handlers
{
    public class GetSearchResultsQueryHandler : IRequestHandler<GetSearchResultsQuery, SearchResult[]>
    {
        public async Task<SearchResult[]> Handle(GetSearchResultsQuery request, CancellationToken cancellationToken)
        {
            return new SearchResult[2] { new SearchResult() { Title = "111" },
                                        new SearchResult() { Title = "222" }};
        }
    }
}
