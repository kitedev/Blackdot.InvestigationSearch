using System;
using System.Collections.Generic;
using System.Text;

namespace Blackdot.InvestigationSearch.Exceptions
{
    public class HtmlParserException : Exception 
    {
        public HtmlParserException(string searchTerm) : base(searchTerm) { }
    }
}
