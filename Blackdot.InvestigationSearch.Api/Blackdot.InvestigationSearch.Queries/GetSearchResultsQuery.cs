using Blackdot.InvestigationSearch.Dtos;
using MediatR;

namespace Blackdot.InvestigationSearch.Queries
{
    public class GetSearchResultsQuery : IRequest<SearchResult[]>
    {
        public string SearchTerm { get; set; }
    }
}
