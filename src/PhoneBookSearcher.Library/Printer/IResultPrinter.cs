using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Printer {
    
    public interface IResultPrinter {

        void Print( List<PhoneBookSearchResult> results );

    }

}
