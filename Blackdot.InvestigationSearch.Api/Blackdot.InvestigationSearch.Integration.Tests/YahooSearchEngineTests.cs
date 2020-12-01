using Blackdot.InvestigationSearch.Core;
using Blackdot.InvestigationSearch.SearchEngines;
using NUnit.Framework;
using System;
using System.Linq;

namespace Blackdot.InvestigationSearch.Integration.Tests
{
    class YahooSearchEngineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void YahooSearchEngine_GetSearchResultsAsync_MultipleResults_HappyPath()
        {
            // arrange
            var searchTerm = "hello";

            var urlSelector = "div.compTitle > h3.title > a";
            var titleSelector = "div.compTitle > h3.title > a";
            var captionSelector = "div.compText > p";
            var searchResultSelector = "div.dd.algo";

            var htmlParser = new AngleSharpHtmlParser();
            var queryStringEncoder = new QueryStringEncoder();

            var yahooEngineSearchSelector = new YahooSearchEngineSelector(urlSelector, titleSelector, captionSelector);

            var searchEngine = new SearchEngine(htmlParser,
                                                yahooEngineSearchSelector,
                                                queryStringEncoder,
                                                "https://uk.search.yahoo.com/search?p=",
                                                searchResultSelector);

            // act
            var results = searchEngine.GetSearchResultsAsync(searchTerm);

            // assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count() > 1);
            Assert.IsTrue(!String.IsNullOrEmpty(results.First().Url));
            Assert.IsTrue(!String.IsNullOrEmpty(results.First().Title));
        }
    }
}
