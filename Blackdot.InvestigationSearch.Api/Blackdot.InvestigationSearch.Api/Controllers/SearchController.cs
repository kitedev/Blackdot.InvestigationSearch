using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blackdot.InvestigationSearch.Api.Dtos;
using Blackdot.InvestigationSearch.Api.Wrapper;
using Blackdot.InvestigationSearch.Exceptions;
using Blackdot.InvestigationSearch.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blackdot.InvestigationSearch.Api.Controllers
{
    /// <summary>
    /// The Search controller containing endpoints for searching for results
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICacheWrapper cacheWrapper;

        public SearchController(IMediator mediator, ICacheWrapper cacheWrapper) 
        {
            this.mediator = mediator;
            this.cacheWrapper = cacheWrapper;
        }


        /// <summary>
        /// Get search results for a given search term.
        /// </summary>
        /// <param name="searchTerm">The search term for finding search results</param>
        /// <returns>An action result containing the answer to the expression</returns>
        /// <response code="200">search results succesfully found</response>
        /// <response code="400">The search term is invalid</response>
        /// <response code="500">An unexpected exception has occured when searching for results</response>
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        [HttpGet]
        public async Task<ActionResult> GetSearchResults([FromQuery(Name = "q")] string searchTerm) 
        {
            if (!this.cacheWrapper.TryGetValue(searchTerm, out List<SearchResponse> searchResponse))
            {
                var query = new GetSearchResultsQuery() { SearchTerm = searchTerm };

                try
                {
                    var searchResults = await mediator.Send(query);
                    searchResponse = searchResults.Select(x => new SearchResponse() { Title = x.Title, Url = x.Url, Caption = x.Caption }).ToList();
                    this.cacheWrapper.Set(searchTerm, searchResponse);
                }
                catch (SearchTermValidationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (AggregateException ae)
                {
                    var errorMessage = "";

                    foreach (var ex in ae.Flatten().InnerExceptions)
                    {
                        errorMessage += ex.Message + Environment.NewLine;
                    }

                    //TODO: log exception details
                    return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
                }
                catch (Exception ex)
                {
                    //TODO: log exception details
                    return StatusCode(StatusCodes.Status500InternalServerError, "There was an unexpected error when searching for the search term. " +
                        "Please contact IT quoting the search term.");
                }
            }

            return Ok(searchResponse);
        }
    }
}
