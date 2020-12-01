using AngleSharp.Dom;

namespace Blackdot.InvestigationSearch.SearchEngines.Interfaces
{
    public interface ISelector
    {
        /// <summary>
        /// The query selector for finding the title
        /// </summary>
        /// <param name="element">The element to find the title</param>
        /// <returns>The title text</returns>
        public string GetTitle(IElement element);

        /// <summary>
        /// The query selector for finding the url
        /// </summary>
        /// <param name="element">The element to find the url</param>
        /// <returns>The url</returns>
        public string GetUrl(IElement element);

        /// <summary>
        /// The query selector for finding the caption
        /// </summary>
        /// <param name="element">The element to find the caption</param>
        /// <returns>The caption text</returns>
        public string GetCaption(IElement element);
    }
}
