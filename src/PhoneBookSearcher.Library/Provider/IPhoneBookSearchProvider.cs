﻿using System;
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
        /// Get all entries by name
        /// </summary>
        /// <param name="query">Name to search for</param>
        /// <returns>List of all results containing searched name.</returns>
        List<PhoneBookSearchResult> GetEntriesByName( string query );

    }
}