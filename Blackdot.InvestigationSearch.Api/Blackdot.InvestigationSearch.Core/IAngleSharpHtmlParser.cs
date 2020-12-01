using AngleSharp.Html.Dom;

namespace Blackdot.InvestigationSearch.Core
{
    public interface IAngleSharpHtmlParser
    {
        /// <summary>
        /// Parse html given a url
        /// </summary>
        /// <param name="url">The url to parse</param>
        /// <returns>AngleSharp HTML Document representing the html from the url</returns>
        IHtmlDocument Parse(string url);
    }
}