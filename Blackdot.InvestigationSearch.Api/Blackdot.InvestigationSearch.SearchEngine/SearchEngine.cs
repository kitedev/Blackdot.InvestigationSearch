﻿using Blackdot.InvestigationSearch.Core;
using Blackdot.InvestigationSearch.Dtos;
using Blackdot.InvestigationSearch.SearchEngines.Interfaces;
using System;
using System.Collections.Generic;

namespace Blackdot.InvestigationSearch.SearchEngines
{
    /// <summary>
    /// The search engine for finding the search results given a search term
    /// </summary>
    public class SearchEngine : ISearchEngine
    {
        private readonly IAngleSharpHtmlParser htmlParser;
        private readonly ISelector searchEngineSelector;
        private readonly IQueryStringEncoder queryStringEncoder;
        private readonly string searchEngineUrl;
        private readonly string searchResultSelector;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="htmlParser">The HTML parser for parsing TML from a URL</param>
        /// <param name="searchEngineSelector">The query selector methods for a given search engine</param>
        /// <param name="queryStringEncoder">For URL encoding</param>
        /// <param name="searchEngineUrl">The search engine URL</param>
        /// <param name="searchResultSelector">The query selector for finding each search result block</param>
        public SearchEngine(IAngleSharpHtmlParser htmlParser, ISelector searchEngineSelector, 
            IQueryStringEncoder queryStringEncoder,
            string searchEngineUrl, string searchResultSelector)
        {
            this.htmlParser = htmlParser;
            this.queryStringEncoder = queryStringEncoder;
            this.searchEngineSelector = searchEngineSelector;
            this.searchEngineUrl = searchEngineUrl;
            this.searchResultSelector = searchResultSelector;
        }

        /// <summary>
        /// Get search results for a given search term
        /// </summary>
        /// <param name="searchTerm">The search term to find search results for</param>
        /// <returns>A collection of search results</returns>
        public IEnumerable<SearchResult> GetSearchResultsAsync(string searchTerm)
        {
            var encodedSearchTerm = this.queryStringEncoder.Encode(searchTerm);

            var uri = this.searchEngineUrl + encodedSearchTerm;

            var document = this.htmlParser.Parse(uri);

            var content = document.QuerySelectorAll(searchResultSelector);

            var searchResults = new List<SearchResult>();

            if (content != null)
            {
                //TODO: parallele forech?
                foreach (var c in content)
                {
                    var url = searchEngineSelector.GetUrl(c);
                    var title = searchEngineSelector.GetTitle(c);
                    var text = searchEngineSelector.GetCaption(c);

                    if (!String.IsNullOrEmpty(url) && !String.IsNullOrEmpty(title))
                    {
                        searchResults.Add(new SearchResult() { Title = title, Caption = text, Url = url });
                    }
                }
            }

            // TODO: throw exception if no results

            return searchResults;

        }
    }
}