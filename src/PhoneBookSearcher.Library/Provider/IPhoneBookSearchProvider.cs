using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Provider {
    
    public interface IPhoneBookSearchProvider {

        List<PhoneBookSearchResult> GetEntriesByName( string query );

    }
}
