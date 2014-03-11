using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library.Provider {
    
    /// <summary>
    /// Interface for all PhoneBookSearch providers
    /// </summary>
    public interface IPhoneBookSearchProvider {

        /// <summary>
        /// Get all entries containing search query
        /// </summary>
        /// <param name="query">Query to search for</param>
        /// <returns>List of all results containing searched query.</returns>
        List<PhoneBookSearchResult> GetEntriesForQuery( string query );

    }
}
