using Blackdot.InvestigationSearch.Core;
using Blackdot.InvestigationSearch.SearchEngines;
using NUnit.Framework;
using System;
using System.Linq;

namespace Blackdot.InvestigationSearch.Integration.Tests
{
    public class BingSearchEngineTests
    {
        public BingSearchEngineTests()
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BingSearchEngine_GetSearchResultsAsync_MultipleResults_HappyPath()
        {
            // arrange
            var searchTerm = "hello";

            var urlSelector = "h2 > a";
            var titleSelector = "h2 > a";
            var captionSelector = "div.b_caption > p";
            var searchResultSelector = "li.b_algo";

            var htmlParserMock = new AngleSharpHtmlParser();
            var queryStringEncoder = new QueryStringEncoder();

            var bingEngineSearchSelector = new BingSearchEngineSelector(urlSelector, titleSelector, captionSelector);

            var searchEngine = new SearchEngine(htmlParserMock,
                                                bingEngineSearchSelector,
                                                queryStringEncoder,
                                                "https://www.bing.com/search?q=",
                                                searchResultSelector);

            // act
            var results = searchEngine.GetSearchResults(searchTerm);

            // assert
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Count() > 1);
            Assert.IsTrue(!String.IsNullOrEmpty(results.First().Url));
            Assert.IsTrue(!String.IsNullOrEmpty(results.First().Title));
        }
    }
}