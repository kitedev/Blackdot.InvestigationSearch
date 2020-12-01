using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
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
            var httpClient = new HttpClient();
            var request = httpClient.GetAsync(url); // TODO: Add async/await

            //Get the response stream
            var response = request.Result.Content.ReadAsStreamAsync().Result; // TODO: Add async/await

            //Parse the stream
            var parser = new HtmlParser();
            return parser.ParseDocument(response);
        }
    }
}
