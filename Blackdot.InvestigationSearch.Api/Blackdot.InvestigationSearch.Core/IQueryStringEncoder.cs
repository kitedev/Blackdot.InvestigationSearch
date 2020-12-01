namespace Blackdot.InvestigationSearch.Core
{
    public interface IQueryStringEncoder
    {
        /// <summary>
        /// For url encoding a string 
        /// </summary>
        /// <param name="queryString">the string to encode</param>
        /// <returns>The URL encoded string</returns>
        string Encode(string queryString);
    }
}
