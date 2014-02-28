using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Printer {
    
    /// <summary>
    /// Class used to print search results to console window
    /// </summary>
    public class ConsoleNameResultPrinter : IResultPrinter {

        #region IResultPrinter methods

        /// <summary>
        /// Prints search results to console window
        /// </summary>
        /// <param name="results">Search results</param>
        public void Print( List<PhoneBookSearchResult> results ) {
            Console.WriteLine( new String( '-', 79 ) );
            foreach (var result in results) {
                Console.WriteLine( "{0,-35}{1,-34}{2,10}",
                    result.FullName,
                    result.MailAddress,
                    result.TelephoneNumber );
            }
            Console.WriteLine( "\nFound {0} result(s)", results.Count );
        }

        #endregion

    }

}
