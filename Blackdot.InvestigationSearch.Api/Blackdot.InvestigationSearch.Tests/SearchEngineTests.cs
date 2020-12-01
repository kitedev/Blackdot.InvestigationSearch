using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Blackdot.InvestigationSearch.Core;
using Blackdot.InvestigationSearch.SearchEngines;
using Blackdot.InvestigationSearch.SearchEngines.Interfaces;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace Blackdot.InvestigationSearch.Tests
{
    public class SearchEngineTests
    {
        //TODO: TESTS TODO
        // failure scenarios
        // where title,url, caption path doesn't exist. Throw exception?

        // write integration test, that calls bing and yahoo search for real

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SearchEngine_GetSearchResultsAsync_SingleResult_HappyPath()
        {
            // arrange
            var searchTerm = "RAF";
            var expectedUrl = "https://www.bbc.co.uk/news";
            var expectedTitle = "RAF cameras capture world's biggest iceberg";
            var expectedCaption = "An RAF aircraft has obtained images of the world's biggest " +
                                  "iceberg as it drifts through the South Atlantic.";

            var testHtml = $"<html><body><div class=\"result\">" +
               $"<a href=\"{expectedUrl}\">{expectedTitle}</a>" +
               $"<p class=\"caption\">{expectedCaption}</p>" +
               $"</div></body></html>";

            var parser = new HtmlParser();
            var document = parser.ParseDocument(testHtml);

            var htmlParserMock = new Mock<IAngleSharpHtmlParser>();
            htmlParserMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(document);

            var queryStringEncoderMock = new Mock<IQueryStringEncoder>();
            queryStringEncoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns(searchTerm);

            var searchEngineSelectorMock = new Mock<ISelector>();
            searchEngineSelectorMock.Setup(x => x.GetUrl(It.IsAny<IElement>())).Returns(expectedUrl);
            searchEngineSelectorMock.Setup(x => x.GetTitle(It.IsAny<IElement>())).Returns(expectedTitle);
            searchEngineSelectorMock.Setup(x => x.GetCaption(It.IsAny<IElement>())).Returns(expectedCaption);

            var searchEngine = new SearchEngine(htmlParserMock.Object,
                                                searchEngineSelectorMock.Object,
                                                queryStringEncoderMock.Object,
                                                "https://test.com/search",
                                                "div.result");

            // act
            var results = searchEngine.GetSearchResultsAsync("RAF");

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(expectedUrl, results.First().Url);
            Assert.AreEqual(expectedTitle, results.First().Title);
            Assert.AreEqual(expectedCaption, results.First().Caption);

        }

        [Test]
        public void SearchEngine_GetSearchResultsAsync_MultipleResults_HappyPath()
        {
            // arrange
            var searchTerm = "test";

            var expectedUrl = "https://test/1/";
            var expectedTitle = "test 1";
            var expectedCaption = "This is the 1st caption";

            var expectedUrl1 = "https://test/2/";
            var expectedTitle1 = "test 2";
            var expectedCaption1 = "This is the 2nd caption";

            var expectedUrl2 = "https://test/3";
            var expectedTitle2 = "test 3";
            var expectedCaption2 = "This is the 3rd caption";

            var testHtml = $"<html><body><div class=\"result\">" +
               $"<a href=\"{expectedUrl}\">{expectedTitle}</a>" +
               $"<p class=\"caption\">{expectedCaption}</p>" +
               $"</div>" +
               $"<div class=\"result\">" +
               $"<a href=\"{expectedUrl1}\">{expectedTitle1}</a>" +
               $"<p class=\"caption\">{expectedCaption1}</p>" +
               $"</div>" +
               $"<div class=\"result\">" +
               $"<a href=\"{expectedUrl2}\">{expectedTitle2}</a>" +
               $"<p class=\"caption\">{expectedCaption2}</p>" +
               $"</div></body></html>";

            var parser = new HtmlParser();
            var document = parser.ParseDocument(testHtml);

            var htmlParserMock = new Mock<IAngleSharpHtmlParser>();
            htmlParserMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(document);

            var queryStringEncoderMock = new Mock<IQueryStringEncoder>();
            queryStringEncoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns(searchTerm);

            var searchEngineSelectorMock = new Mock<ISelector>();
            searchEngineSelectorMock.SetupSequence(x => x.GetUrl(It.IsAny<IElement>()))
                                    .Returns(expectedUrl)
                                    .Returns(expectedUrl1)
                                    .Returns(expectedUrl2);

            searchEngineSelectorMock.SetupSequence(x => x.GetTitle(It.IsAny<IElement>()))
                                    .Returns(expectedTitle)
                                    .Returns(expectedTitle1)
                                    .Returns(expectedTitle2);

            searchEngineSelectorMock.SetupSequence(x => x.GetCaption(It.IsAny<IElement>()))
                                    .Returns(expectedCaption)
                                    .Returns(expectedCaption1)
                                    .Returns(expectedCaption2);

            var searchEngine = new SearchEngine(htmlParserMock.Object,
                                                searchEngineSelectorMock.Object,
                                                queryStringEncoderMock.Object,
                                                "https://test.com/search",
                                                "div.result");

            // act
            var results = searchEngine.GetSearchResultsAsync(searchTerm).ToList();

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count());

            Assert.AreEqual(expectedUrl, results[0].Url);
            Assert.AreEqual(expectedTitle, results[0].Title);
            Assert.AreEqual(expectedCaption, results[0].Caption);

            Assert.AreEqual(expectedUrl1, results[1].Url);
            Assert.AreEqual(expectedTitle1, results[1].Title);
            Assert.AreEqual(expectedCaption1, results[1].Caption);

            Assert.AreEqual(expectedUrl2, results[2].Url);
            Assert.AreEqual(expectedTitle2, results[2].Title);
            Assert.AreEqual(expectedCaption2, results[2].Caption);

        }

        [Test]
        public void SearchEngine_GetSearchResultsAsync_SingleResult2_HappyPath()
        {
            // arrange
            var searchTerm = "RAF";
            var expectedUrl = "https://www.bbc.co.uk/news";
            var expectedTitle = "RAF cameras capture world's biggest iceberg";
            var expectedCaption = "An RAF aircraft has obtained images of the world's biggest " +
                                  "iceberg as it drifts through the South Atlantic.";

            var testHtml = $"<html><body><div class=\"result\">" +
               $"<p class=\"url\"\">{expectedUrl}\"</p>" +
               $"<p class=\"title\">{expectedTitle}</p>" +
               $"<p class=\"caption\">{expectedCaption}</p>" +
               $"</div></body></html>";

            var parser = new HtmlParser();
            var document = parser.ParseDocument(testHtml);

            var htmlParserMock = new Mock<IAngleSharpHtmlParser>();
            htmlParserMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(document);

            var queryStringEncoderMock = new Mock<IQueryStringEncoder>();
            queryStringEncoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns(searchTerm);

            var searchEngineSelectorMock = new Mock<ISelector>();
            searchEngineSelectorMock.Setup(x => x.GetUrl(It.IsAny<IElement>())).Returns(expectedUrl);
            searchEngineSelectorMock.Setup(x => x.GetTitle(It.IsAny<IElement>())).Returns(expectedTitle);
            searchEngineSelectorMock.Setup(x => x.GetCaption(It.IsAny<IElement>())).Returns(expectedCaption);

            var searchEngine = new SearchEngine(htmlParserMock.Object,
                                                searchEngineSelectorMock.Object,
                                                queryStringEncoderMock.Object,
                                                "https://test.com/search",
                                                "div.result");

            // act
            var results = searchEngine.GetSearchResultsAsync(searchTerm);

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(expectedUrl, results.First().Url);
            Assert.AreEqual(expectedTitle, results.First().Title);
            Assert.AreEqual(expectedCaption, results.First().Caption);

        }

        [Test]
        public void SearchEngine_GetSearchResultsAsync_UrlMissing_NoResult()
        {
            // arrange
            var searchTerm = "RAF";
            var expectedTitle = "RAF cameras capture world's biggest iceberg";
            var expectedCaption = "An RAF aircraft has obtained images of the world's biggest " +
                                  "iceberg as it drifts through the South Atlantic.";

            var testHtml = $"<html><body><div class=\"result\">" +
               $"<p class=\"title\">{expectedTitle}</p>" +
               $"<p class=\"caption\">{expectedCaption}</p>" +
               $"</div></body></html>";

            var parser = new HtmlParser();
            var document = parser.ParseDocument(testHtml);

            var htmlParserMock = new Mock<IAngleSharpHtmlParser>();
            htmlParserMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(document);

            var queryStringEncoderMock = new Mock<IQueryStringEncoder>();
            queryStringEncoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns(searchTerm);

            var searchEngineSelectorMock = new Mock<ISelector>();
            searchEngineSelectorMock.Setup(x => x.GetUrl(It.IsAny<IElement>())).Returns((string)null);
            searchEngineSelectorMock.Setup(x => x.GetTitle(It.IsAny<IElement>())).Returns(expectedTitle);
            searchEngineSelectorMock.Setup(x => x.GetCaption(It.IsAny<IElement>())).Returns(expectedCaption);

            var searchEngine = new SearchEngine(htmlParserMock.Object,
                                                searchEngineSelectorMock.Object,
                                                queryStringEncoderMock.Object,
                                                "https://test.com/search",
                                                "div.result");

            // act
            var results = searchEngine.GetSearchResultsAsync(searchTerm);

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());

        }

        [Test]
        public void SearchEngine_GetSearchResultsAsync_TitleMissing_NoResult()
        {
            // arrange
            var searchTerm = "RAF";
            var expectedUrl = "https://www.bbc.co.uk/news";
            var expectedCaption = "An RAF aircraft has obtained images of the world's biggest " +
                                  "iceberg as it drifts through the South Atlantic.";

            var testHtml = $"<html><body><div class=\"result\">" +
               $"<p class=\"url\"\">{expectedUrl}\"</p>" +
               $"<p class=\"caption\">{expectedCaption}</p>" +
               $"</div></body></html>";

            var parser = new HtmlParser();
            var document = parser.ParseDocument(testHtml);

            var htmlParserMock = new Mock<IAngleSharpHtmlParser>();
            htmlParserMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(document);

            var queryStringEncoderMock = new Mock<IQueryStringEncoder>();
            queryStringEncoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns(searchTerm);

            var searchEngineSelectorMock = new Mock<ISelector>();
            searchEngineSelectorMock.Setup(x => x.GetUrl(It.IsAny<IElement>())).Returns(expectedUrl);
            searchEngineSelectorMock.Setup(x => x.GetTitle(It.IsAny<IElement>())).Returns((string)null);
            searchEngineSelectorMock.Setup(x => x.GetCaption(It.IsAny<IElement>())).Returns(expectedCaption);

            var searchEngine = new SearchEngine(htmlParserMock.Object,
                                                searchEngineSelectorMock.Object,
                                                queryStringEncoderMock.Object,
                                                "https://test.com/search",
                                                "div.result");

            // act
            var results = searchEngine.GetSearchResultsAsync(searchTerm);

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count());

        }

        [Test]
        public void SearchEngine_GetSearchResultsAsync_CaptionMissing_OneResult()
        {
            // arrange
            var searchTerm = "RAF";
            var expectedUrl = "https://www.bbc.co.uk/news";
            var expectedTitle = "RAF cameras capture world's biggest iceberg";

            var testHtml = $"<html><body><div class=\"result\">" +
               $"<p class=\"url\"\">{expectedUrl}\"</p>" +
               $"<p class=\"title\">{expectedTitle}</p>" +
               $"</div></body></html>";

            var parser = new HtmlParser();
            var document = parser.ParseDocument(testHtml);

            var htmlParserMock = new Mock<IAngleSharpHtmlParser>();
            htmlParserMock.Setup(x => x.Parse(It.IsAny<string>())).Returns(document);

            var queryStringEncoderMock = new Mock<IQueryStringEncoder>();
            queryStringEncoderMock.Setup(x => x.Encode(It.IsAny<string>())).Returns(searchTerm);

            var searchEngineSelectorMock = new Mock<ISelector>();
            searchEngineSelectorMock.Setup(x => x.GetUrl(It.IsAny<IElement>())).Returns(expectedUrl);
            searchEngineSelectorMock.Setup(x => x.GetTitle(It.IsAny<IElement>())).Returns(expectedTitle);
            searchEngineSelectorMock.Setup(x => x.GetCaption(It.IsAny<IElement>())).Returns((string)null);

            var searchEngine = new SearchEngine(htmlParserMock.Object,
                                                searchEngineSelectorMock.Object,
                                                queryStringEncoderMock.Object,
                                                "https://test.com/search",
                                                "div.result");

            // act
            var results = searchEngine.GetSearchResultsAsync(searchTerm);

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(expectedUrl, results.First().Url);
            Assert.AreEqual(expectedTitle, results.First().Title);
        }

    }
}