using System;

namespace Blackdot.InvestigationSearch.Exceptions
{
    public class SearchTermValidationException : Exception
    {
        public SearchTermValidationException(string searchTerm) : base(searchTerm) { }
}
}
