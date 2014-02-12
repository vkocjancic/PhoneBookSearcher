using PhoneBookSearcher.Library.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookSearcher.Library {
    
    public sealed class PhoneBookSearch {

        #region Declarations

        public IPhoneBookSearchProvider Provider { get; private set; }

        #endregion

        #region Constructors

        public PhoneBookSearch( IPhoneBookSearchProvider provider ) {
            if (null == provider)
                throw new ArgumentNullException( "Provider" );
            this.Provider = provider;
        }

        #endregion

        #region Public methods

        public List<PhoneBookSearchResult> Search( PhoneBookQuery query ) {
            if (null == query)
                throw new ArgumentNullException( "Query" );
            return this.Provider.GetEntriesByName( query.PersonName );
        }

        #endregion

    }

}
