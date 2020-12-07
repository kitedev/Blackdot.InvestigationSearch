using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Blackdot.InvestigationSearch.Exceptions;
using System;
using System.Net.Http;

namespace Blackdot.InvestigationSearch.Core
{
    /// <summary>
    /// For parsing html using AngleSharp
    /// </summary>
    public class AngleSharpHtmlParser : IAngleSharpHtmlParser
    {
        /// <summary>
        /// Parse html given a url
        /// </summary>
        /// <param name="url">The url to parse</param>
        /// <returns>AngleSharp HTML Document representing the html from the url</returns>
        public IHtmlDocument Parse(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                var request = httpClient.GetAsync(url); // TODO: Add async/await

                //Get the response stream
                var response = request.Result.Content.ReadAsStreamAsync().Result; // TODO: Add async/await

                //Parse the stream
                var parser = new HtmlParser();
                return parser.ParseDocument(response);
            }
            catch (System.ArgumentNullException ex)
            {
                throw new HtmlParserException("Url argument was null. Check search engine url configurations in appsettings.json");
            }
            catch (HttpRequestException ex)
            {
                throw new HtmlParserException($"Unable to make a request to the specified url ({url}). Please check the url or internet connectivity.");
            }
            catch (Exception ex) 
            {
                throw new HtmlParserException($"An unexpected error occured when making a request to the url: {url}");
            }
        }
    }
}
