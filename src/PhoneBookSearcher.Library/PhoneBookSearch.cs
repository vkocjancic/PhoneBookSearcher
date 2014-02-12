using PhoneBookSearcher.Library.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library {
    
    /// <summary>
    /// Class that perfroms PhoneBookSearch
    /// </summary>
    public sealed class PhoneBookSearch {

        #region Declarations

        /// <summary>
        /// Gets PhoneBookSearch provider object
        /// </summary>
        public IPhoneBookSearchProvider Provider { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider">PhoneBookSearchProvider object</param>
        public PhoneBookSearch( IPhoneBookSearchProvider provider ) {
            if (null == provider)
                throw new ArgumentNullException( "Provider" );
            this.Provider = provider;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Performs search using provider passed with constructor and PhoneBookQuery object
        /// </summary>
        /// <param name="query">PhoneBookQuery object which contains query(ies) to execute</param>
        /// <returns></returns>
        public List<PhoneBookSearchResult> Search( PhoneBookQuery query ) {
            if (null == query)
                throw new ArgumentNullException( "Query" );
            return this.Provider.GetEntriesByName( query.PersonName );
        }

        #endregion

    }

}
