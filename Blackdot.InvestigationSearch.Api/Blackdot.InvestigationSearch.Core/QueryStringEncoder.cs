using System.Web;

namespace Blackdot.InvestigationSearch.Core
{
    /// <summary>
    /// For URL encoding a query string
    /// </summary>
    public class QueryStringEncoder : IQueryStringEncoder
    {
        /// <summary>
        /// For url encoding a string 
        /// </summary>
        /// <param name="queryString">the string to encode</param>
        /// <returns>The URL encoded string</returns>
        public string Encode(string searchTerm)
        {
            return HttpUtility.UrlEncode(searchTerm);
        }
    }
}
