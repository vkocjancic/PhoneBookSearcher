using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Printer {
    
    /// <summary>
    /// Interface for all result printers
    /// </summary>
    public interface IResultPrinter {

        /// <summary>
        /// Prints PhoneBookSearch results
        /// </summary>
        /// <param name="results">PhoneBookSearch results to print</param>
        void Print( List<PhoneBookSearchResult> results );

    }

}
