using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Blackdot.InvestigationSearch.SearchEngines.Interfaces;
using System.Linq;

namespace Blackdot.InvestigationSearch.SearchEngines
{
    /// <summary>
    /// Provide query selector methods for the Yahoo search engine
    /// </summary>
    public class YahooSearchEngineSelector : ISelector
    {
        private readonly string urlSelector;
        private readonly string titleSelector;
        private readonly string captionSelector;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="urlSelector">The query selector for find the url</param>
        /// <param name="titleSelector">The query selector for find the title</param>
        /// <param name="captionSelector">The query selector for find the caption</param>
        public YahooSearchEngineSelector(string urlSelector, string titleSelector, string captionSelector)
        {
            this.urlSelector = urlSelector;
            this.titleSelector = titleSelector;
            this.captionSelector = captionSelector;
        }

        /// <summary>
        /// The query selector for finding the caption
        /// </summary>
        /// <param name="element">The element to find the caption</param>
        /// <returns>The caption text</returns>
        public string GetCaption(IElement element)
        {
            return ((IHtmlParagraphElement)element.QuerySelectorAll(this.captionSelector).FirstOrDefault())?.InnerHtml;
        }

        /// <summary>
        /// The query selector for finding the title
        /// </summary>
        /// <param name="element">The element to find the title</param>
        /// <returns>The title text</returns>
        public string GetTitle(IElement element)
        {
            return ((IHtmlAnchorElement)element.QuerySelectorAll(this.titleSelector).FirstOrDefault())?.InnerHtml;
        }

        /// <summary>
        /// The query selector for finding the url
        /// </summary>
        /// <param name="element">The element to find the url</param>
        /// <returns>The url</returns>
        public string GetUrl(IElement element)
        {
            return ((IHtmlAnchorElement)element.QuerySelectorAll(this.urlSelector).FirstOrDefault())?.Href;
        }
    }
}
