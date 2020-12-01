using Blackdot.InvestigationSearch.Api.Controllers;
using Blackdot.InvestigationSearch.Api.Dtos;
using Blackdot.InvestigationSearch.Api.Wrapper;
using Blackdot.InvestigationSearch.Core;
using Blackdot.InvestigationSearch.Queries.Handlers;
using Blackdot.InvestigationSearch.SearchEngines;
using Blackdot.InvestigationSearch.SearchEngines.Interfaces;
using Blackdot.InvestigationSearch.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blackdot.InvestigationSearch.Integration.Tests
{
    class SearchControllerTests
    {
        private IMediator mediator;

        [SetUp]
        public void Init() 
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IAngleSharpHtmlParser, AngleSharpHtmlParser>();
            services.AddSingleton<IQueryStringEncoder, QueryStringEncoder>();

            var provider = services.BuildServiceProvider();
            var htmlParser = provider.GetService<IAngleSharpHtmlParser>();
            var queryStringEncoder = provider.GetService<IQueryStringEncoder>();

            var bingSelector = new BingSearchEngineSelector(config.GetValue<string>("BingUrlSelector"),
                                                                    config.GetValue<string>("BingTitleSelector"),
                                                                    config.GetValue<string>("BingCaptionSelector"));

            var yahooSelector = new YahooSearchEngineSelector(config.GetValue<string>("YahooUrlSelector"),
                                                        config.GetValue<string>("YahooTitleSelector"),
                                                        config.GetValue<string>("YahooCaptionSelector"));

            // Bing search
            services.AddSingleton<ISearchEngine>(new SearchEngine(htmlParser,
                                                                  bingSelector,
                                                                  queryStringEncoder,
                                                                  config.GetValue<string>("BingUrl"),
                                                                  config.GetValue<string>("BingSearchResultSelector")
                                                                  ));

            // Yahoo search
            services.AddSingleton<ISearchEngine>(new SearchEngine(htmlParser,
                                                                  yahooSelector,
                                                                  queryStringEncoder,
                                                                  config.GetValue<string>("YahooUrl"),
                                                                  config.GetValue<string>("YahooSearchResultSelector")));

            services.AddMediatR(Assembly.GetExecutingAssembly(),
                                Assembly.GetAssembly(typeof(GetSearchResultsQueryHandler)));

            services.AddSingleton<IValidator<string>, SearchTermValidator>();

            provider = services.BuildServiceProvider();
            this.mediator = provider.GetService<IMediator>();
        }

        [Test]
        [TestCase("hello")]
        [TestCase("SpaceX")]
        [TestCase("quantum physics")]
        [TestCase("lol")]
        [TestCase("support vector machines")]
        [TestCase("To Be or Not To Be")]
        [TestCase("Hello, how are you?")]
        [TestCase("1+2+3")]
        [TestCase("1+2+3*5")]
        [TestCase("1+2+3*5/2")]
        [TestCase("1+2+(2/2)")]
        public async System.Threading.Tasks.Task SearchController_GetSearchResults_HappyPathAsync(string searchTerm)
        {
            // arrange
            var cacheMock = new Mock<ICacheWrapper>();
            var cacheEntry = Mock.Of<ICacheEntry>();

            cacheMock.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<List<SearchResponse>>()));

            var controller = new SearchController(this.mediator, cacheMock.Object);

            //act
            var response = await controller.GetSearchResults(searchTerm);

            //assert
            Assert.IsNotNull(response);
            var results = (List<SearchResponse>)((OkObjectResult)response).Value;

            Assert.IsTrue(results.Count() > 1);
            Assert.IsTrue(!String.IsNullOrEmpty(results.First().Url));
            Assert.IsTrue(!String.IsNullOrEmpty(results.First().Title));
        }

        [Test]
        [TestCase("a", "Please provide a search term with a minimum of 3 characters.")]
        [TestCase("12", "Please provide a search term with a minimum of 3 characters.")]
        [TestCase("", "Please provide a search term. A search term cannot be null or empty.\r\nPlease provide a search term with a minimum of 3 characters.")]
        public async System.Threading.Tasks.Task SearchController_GetSearchResults_InvalidRequest(string searchTerm, string expectedMessage)
        {
            // arrange
            var cacheMock = new Mock<ICacheWrapper>();
            var cacheEntry = Mock.Of<ICacheEntry>();

            cacheMock.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<List<SearchResponse>>()));

            var controller = new SearchController(this.mediator, cacheMock.Object);

            //act
            var response = await controller.GetSearchResults(searchTerm);

            //assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), response);
            Assert.That(((BadRequestObjectResult)response).Value, Is.EqualTo(expectedMessage));
        }
    }
}
