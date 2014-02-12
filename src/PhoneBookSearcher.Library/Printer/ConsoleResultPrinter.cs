using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Printer {
    
    public class ConsoleResultPrinter : IResultPrinter {

        #region IResultPrinter methods

        public void Print( List<PhoneBookSearchResult> results ) {
            Console.WriteLine( "Found {0} result(s)", results.Count );
            if (0 != results.Count)
                Console.WriteLine( new String( '-', 79 ) );
            foreach (var result in results) {
                Console.WriteLine( "{0,-35}{1,-34}{2,10}",
                    result.FullName,
                    result.MailAddress,
                    result.TelephoneNumber );
            }
        }

        #endregion

    }

}
